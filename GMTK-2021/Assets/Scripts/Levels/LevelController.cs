using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Levels;
using LogicalElements;
using Ui;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public PostEffect RewindEffect;
    public PlayerController Player;
    public PlaybackDummy PlayerDummy;
    public GameLevel[] Levels;
    public Text ParadoxText;
    
    [SerializeField] private LevelUiController _levelUi;
    [SerializeField] private MusicProgressionController _musicProgressionController;

    [SerializeField] private AudioSource _rewindLevelSource;
    [SerializeField] private AudioSource _rewindToBackSource;
    [SerializeField] private AudioSource _paradoxSource;
    [SerializeField] private TimelockUi _timelockUi;
    
    public int StartingLevel;
    public float TransitionDuration = 0.75F;
    
    public bool HasProgressedAtLeastOnce { get; private set; }
    public bool IsDummyCompleted { get; set; }
    public bool IsPlayerCompleted { get; set; }

    private int _currentLevel = -1;
    private int _pastLevel = -1;
    private bool _isTransitioning;
    private PostEffect _glitchEffect;
    private PostEffect _timestopEffect;
    private Rigidbody2D _playerRb;
    private Rigidbody2D _dummyRb;

    public bool HasPastLevel => _pastLevel != -1;
    public GameLevel CurrentLevel => _currentLevel < Levels.Length ? Levels[_currentLevel] : null;
    public GameLevel PastLevel => _pastLevel >= 0 ? Levels[_pastLevel] : null;

    private LevelActivatablesController _levelActivatablesController;

    private void Awake()
    {
        #if UNITY_EDITOR
        Application.targetFrameRate = 0;
        #endif

        RewindEffect = RewindEffect.GetComponents<PostEffect>()[1];
        _glitchEffect = RewindEffect.GetComponents<PostEffect>()[2];
        _timestopEffect = RewindEffect.GetComponents<PostEffect>()[3];
    }

    private void Start()
    {
        TransitionToLevel(StartingLevel, true);
        _levelUi.InitLevelUi();
        
        Player.ReassignToLevel(CurrentLevel);
        PlayerDummy.Player.ReassignToLevel(CurrentLevel);

        _playerRb = Player.GetComponent<Rigidbody2D>();
        _dummyRb = PlayerDummy.GetComponent<Rigidbody2D>();
    }

    public void Timelock()
    {
        if (_pastLevel >= 0)
        {
            var timeLocked = !PastLevel.Timeline.IsTimelocked;
            PastLevel.Timeline.IsTimelocked = timeLocked;
            PlayerDummy.Timelock();
            if (timeLocked)
                _timelockUi.TimelockOn();
            else 
                _timelockUi.TimelockOff();
        }
    }
    
    public void ReloadLevel()
    {
        _musicProgressionController.SetLoopPhase(_currentLevel);
        _musicProgressionController.Play();
        
        if (_pastLevel >= 0)
        {
            _levelUi.Enable();
            _levelUi.OnNextLevelStart(PastLevel, CurrentLevel);
            PastLevel.LoadLevelInitialState();
        }
        else
        {
            _levelUi.Disable();
        }
        
        CurrentLevel.LoadLevelInitialState();
        CurrentLevel.AssignObjectAndSpawnAtStart(Player.gameObject);
        CurrentLevel.Timeline.ClearTimeline();

        PlayerDummy.RespawnDummy();
        
        Player.gameObject.SetActive(true);
        var rb = Player.GetComponent<Rigidbody2D>();
        rb.simulated = true;
        rb.velocity = Vector2.zero;

        IsDummyCompleted = false;
        IsPlayerCompleted = false;
        
        _levelActivatablesController?.UnSubscribeFromActivatorEvents();
        _levelActivatablesController = new LevelActivatablesController(PastLevel, CurrentLevel);
        
        print($"past: {_pastLevel} current: {_currentLevel}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) && HasPastLevel && !_isTransitioning)
        {
            _rewindToBackSource.Play();
            TransitionToPrevLevel(true);
        }

        if (Input.GetKeyDown(KeyCode.R) && !_isTransitioning)
        {
            _rewindLevelSource.Play();
            Player.GetComponent<Rigidbody2D>().simulated = false;
            _isTransitioning = true;
            StartCoroutine(PlayRewindAnimation());
        }

        if (!_dummyRb.simulated && !_isTransitioning && HasPastLevel)
            _timestopEffect.Strength = Mathf.Min(1F, _timestopEffect.Strength + Time.deltaTime * 3F);
        else
            _timestopEffect.Strength = Mathf.Max(0F, _timestopEffect.Strength - Time.deltaTime * 3F);
        
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F5))
            TransitionToNextLevel();
        
        if (Input.GetKeyDown(KeyCode.F6))
            TransitionToPrevLevel();
        #endif
                
        if (PastLevel != null)
            _levelUi.UpdateUi(PastLevel.Timeline.CurrentTick);
    }

    public void TransitionToNextLevel()
    {
        Player.ReassignToLevel(Levels[_currentLevel + 1]);
        TransitionToLevel(_currentLevel + 1);
    }
    
    public void TransitionToPrevLevel(bool playEffect = false)
    {
        Player.ReassignToLevel(Levels[_currentLevel - 1]);
        TransitionToLevel(_currentLevel - 1, playEffect: playEffect);
    }

    public void TransitionToLevel(int levelId, bool skipAnim = false, bool playEffect = false)
    {
        if (levelId == Levels.Length)
            SceneManager.LoadScene(3);
        
        _isTransitioning = true;
        IsDummyCompleted = false;
        IsPlayerCompleted = false;
        StartCoroutine(PlayTransitionAnimation(levelId, skipAnim, playEffect, playEffect ? 1F : 0F));
    }

    public void CauseParadox()
    {
        if (_isTransitioning)
            return;

        _isTransitioning = true;
        Player.GetComponent<Rigidbody2D>().simulated = false;
        _paradoxSource.Play();
        StartCoroutine(PlayParadoxAnimation());
    }

    public void CauseDeath()
    {
        if (_isTransitioning)
            return;

        _isTransitioning = true;
        Player.GetComponent<Rigidbody2D>().simulated = false;
        _rewindLevelSource.Play();
        StartCoroutine(PlayDeathAnimation());
    }
    
    private IEnumerator PlayDeathAnimation()
    {
        _levelUi.Disable();
        
        var animStart = Time.timeSinceLevelLoad;
        
        RewindEffect.Intensity = 0.4F;
        RewindEffect.enabled = true;

        ParadoxText.text = "DEAD";
        ParadoxText.gameObject.SetActive(true);

        while (Time.timeSinceLevelLoad - animStart < 2.5F)
        {
            RewindEffect.Intensity = 
                1F + Mathf.Clamp((Time.timeSinceLevelLoad - animStart) / 0.2F * 2F, 0F, 2F)
                - Mathf.Clamp((Time.timeSinceLevelLoad - animStart - 0.1F) / 0.2F * 2F, 0F, 2F) +
                Mathf.Clamp((Time.timeSinceLevelLoad - animStart) / 2.5F * 3.5F, 0F, 3.5F);
            
            RewindEffect.Strength = 1F - Mathf.Clamp(
                (Time.timeSinceLevelLoad - animStart - 2.3F) / 0.2F, 0F, 1F);

            yield return null;
        }
        
        ParadoxText.gameObject.SetActive(false);
        
        RewindEffect.enabled = false;

        _isTransitioning = false;
        
        ReloadLevel();
    }
    
    private IEnumerator PlayParadoxAnimation()
    {
        _levelUi.Disable();
        
        var animStart = Time.timeSinceLevelLoad;
        
        RewindEffect.Intensity = 0.4F;
        RewindEffect.enabled = true;

        _glitchEffect.enabled = true;
        
        ParadoxText.text = "PARADOX!";
        ParadoxText.gameObject.SetActive(true);

        while (Time.timeSinceLevelLoad - animStart < 2.5F)
        {
            RewindEffect.Intensity = 
                1F + Mathf.Clamp((Time.timeSinceLevelLoad - animStart) / 0.2F * 2F, 0F, 2F)
                - Mathf.Clamp((Time.timeSinceLevelLoad - animStart - 0.1F) / 0.2F * 2F, 0F, 2F) +
                Mathf.Clamp((Time.timeSinceLevelLoad - animStart) / 2.5F * 3.5F, 0F, 3.5F);

            _glitchEffect.Intensity = Mathf.Clamp(Time.timeSinceLevelLoad - animStart - 1.5F, 0F, 1F);
            
            RewindEffect.Strength = 1F - Mathf.Clamp(
                (Time.timeSinceLevelLoad - animStart - 2.3F) / 0.2F, 0F, 1F);

            yield return null;
        }
        
        ParadoxText.gameObject.SetActive(false);
        
        _glitchEffect.enabled = false;
        RewindEffect.enabled = false;

        _isTransitioning = false;
        
        ReloadLevel();
    }

    private IEnumerator PlayRewindAnimation()
    {
        var animStart = Time.timeSinceLevelLoad;

        RewindEffect.Intensity = 0.4F;
        RewindEffect.enabled = true;

        var playerPos = Player.transform.localPosition;
        var dummyPos = PlayerDummy.transform.localPosition;

        while (Time.timeSinceLevelLoad - animStart < 0.75F)
        {
            RewindEffect.Strength = Mathf.Clamp((Time.timeSinceLevelLoad - animStart) / 0.1F, 0F, 1F) -
                                    Mathf.Clamp((Time.timeSinceLevelLoad - animStart - 0.65F) / 0.1F, 0F, 1F);
            
            Player.transform.localPosition = Vector3.Lerp(playerPos, 
                CurrentLevel.PlayerStart.transform.localPosition, (Time.timeSinceLevelLoad - animStart) / 0.75F);

            if (HasPastLevel)
            {
                PlayerDummy.transform.localPosition = Vector3.Lerp(dummyPos,
                    PastLevel.PlayerStart.transform.localPosition, (Time.timeSinceLevelLoad - animStart) / 0.75F);
            }

            yield return null;
        }

        _isTransitioning = false;
        RewindEffect.enabled = false;
        ReloadLevel();
    }

    private IEnumerator PlayTransitionAnimation(int nextLevelId, bool skipAnim, bool playEffect, float transitionDuration = 0F)
    {
        _levelUi.Disable();
        
        var animStart = Time.timeSinceLevelLoad;
        var returnBack = nextLevelId == _pastLevel;

        if (transitionDuration == 0F)
            transitionDuration = TransitionDuration;

        var pastLevel = returnBack ? Levels[_currentLevel] :
            _pastLevel == -1 ? null : Levels[_pastLevel];
        var currentLevel = returnBack ? Levels[_pastLevel] :
            _currentLevel == -1 ? null : Levels[_currentLevel];
        var futureLevel = returnBack 
            ? _pastLevel != 0 ? Levels[_pastLevel - 1] : null 
            : Levels[nextLevelId];

        if (!returnBack || _pastLevel != 0)
        {
            futureLevel.gameObject.SetActive(true);

            futureLevel.transform.position = returnBack ? new Vector3(0F, -13.5F) : new Vector3(0F, 13.5F);
        }

        if (playEffect)
        {
            RewindEffect.Intensity = 1F;
            RewindEffect.Strength = 1F;
            RewindEffect.enabled = true;
        }

        if (!skipAnim)
        {
            if (!returnBack)
            {
                while (Time.timeSinceLevelLoad - animStart < transitionDuration)
                {
                    if (_pastLevel != -1)
                    {
                        pastLevel.transform.position =
                            new Vector3(0F, -4.5F - (Time.timeSinceLevelLoad - animStart) / transitionDuration * 9F);
                        currentLevel.transform.position =
                            new Vector3(0F, 4.5F - (Time.timeSinceLevelLoad - animStart) / transitionDuration * 9F);
                    }
                    else
                    {
                        currentLevel.transform.position =
                            new Vector3(0F, -(Time.timeSinceLevelLoad - animStart) / transitionDuration * 4.5F);
                    }

                    futureLevel.transform.position =
                        new Vector3(0F, 13.5F - (Time.timeSinceLevelLoad - animStart) / transitionDuration * 9F);

                    yield return null;
                }
            }
            else
            {
                while (Time.timeSinceLevelLoad - animStart < transitionDuration)
                {
                    if (playEffect)
                    {
                        RewindEffect.Intensity = 
                            1F + Mathf.Clamp((Time.timeSinceLevelLoad - animStart) / (transitionDuration * 0.1F) * 2F, 0F, 2F)
                            - Mathf.Clamp((Time.timeSinceLevelLoad - animStart - 0.1F) / (transitionDuration * 0.1F) * 2F, 0F, 2F);
                        
                        RewindEffect.Strength = 1F - Mathf.Clamp(
                            (Time.timeSinceLevelLoad - animStart - (transitionDuration - transitionDuration * 0.1F)) / (transitionDuration * 0.1F), 0F, 1F);
                    }
                    
                    pastLevel.transform.position =
                        new Vector3(0F, 4.5F + (Time.timeSinceLevelLoad - animStart) / transitionDuration * 9F);

                    if (_pastLevel != 0)
                    {
                        currentLevel.transform.position =
                            new Vector3(0F, -4.5F + (Time.timeSinceLevelLoad - animStart) / transitionDuration * 9F);
                        futureLevel.transform.position =
                            new Vector3(0F, -13.5F + (Time.timeSinceLevelLoad - animStart) / transitionDuration * 9F);
                    }
                    else
                    {
                        currentLevel.transform.position =
                            new Vector3(0F, -4.5F + (Time.timeSinceLevelLoad - animStart) / transitionDuration * 4.5F);
                    }

                    yield return null;
                }
            }
        }

        if (!returnBack)
        {
            if (_pastLevel != -1)
                pastLevel.transform.position = new Vector3(0F, -13.5F);

            if (_currentLevel != -1)
            {
                currentLevel.transform.position = new Vector3(0F, -4.5F);
                futureLevel.transform.position = new Vector3(0F, 4.5F);
            }
            else
            {
                futureLevel.transform.position = new Vector3(0F, 0F);
            }
        }
        else
        {
            pastLevel.transform.position = new Vector3(0F, 13.5F);

            if (_pastLevel != 0)
            {
                currentLevel.transform.position = new Vector3(0F, 4.5F);
                futureLevel.transform.position = new Vector3(0F, -4.5F);
            }
            else
            {
                currentLevel.transform.position = new Vector3(0F, 0F);
            }
        }

        if (_pastLevel != -1)
            pastLevel.gameObject.SetActive(false);

        if (!returnBack)
        {
            _pastLevel = _currentLevel;
            _currentLevel = nextLevelId;
        }
        else
        {
            _pastLevel -= 1;
            _currentLevel = nextLevelId;
        }

        if (playEffect)
            RewindEffect.enabled = false;

        ReloadLevel();
        _isTransitioning = false;

        if (!skipAnim)
            HasProgressedAtLeastOnce = true;
    }
}