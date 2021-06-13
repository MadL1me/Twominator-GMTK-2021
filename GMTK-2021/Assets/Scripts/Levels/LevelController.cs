using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Levels;
using LogicalElements;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public PostEffect RewindEffect;
    public PlayerController Player;
    public PlaybackDummy PlayerDummy;
    public GameLevel[] Levels;
    
    [SerializeField] private LevelUiController _levelUi;
    [SerializeField] private MusicProgressionController _musicProgressionController;
    
    [SerializeField] private AudioSource _rewindLevelSource;
    [SerializeField] private AudioSource _rewindToBackSource;
    
    public int StartingLevel;
    public float TransitionDuration = 0.75F;
    
    public bool HasProgressedAtLeastOnce { get; private set; }
    public bool IsDummyCompleted { get; set; }
    public bool IsPlayerCompleted { get; set; }

    private int _currentLevel = -1;
    private int _pastLevel = -1;
    private bool _isTransitioning;

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
    }

    private void Start()
    {
        TransitionToLevel(StartingLevel, true);
        _levelUi.InitLevelUi();
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
        Player.GetComponent<Rigidbody2D>().simulated = true;

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
            TransitionToPrevLevel();
        }

        if (Input.GetKeyDown(KeyCode.R) && !_isTransitioning)
        {
            _rewindLevelSource.Play();
            Player.GetComponent<Rigidbody2D>().simulated = false;
            StartCoroutine(PlayRewindAnimation());
        }
        
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F5))
            TransitionToNextLevel();
        
        if (Input.GetKeyDown(KeyCode.F6))
            TransitionToPrevLevel();
        #endif
                
        _levelUi.UpdateUi(CurrentLevel.Timeline.CurrentTick);
    }

    public void TransitionToNextLevel()
    {
        Player.ReassignToLevel(Levels[_currentLevel + 1]);
        TransitionToLevel(_currentLevel + 1);
    }
    
    public void TransitionToPrevLevel()
    {
        Player.ReassignToLevel(Levels[_currentLevel - 1]);
        TransitionToLevel(_currentLevel - 1);
    }

    public void TransitionToLevel(int levelId, bool skipAnim = false)
    {
        _isTransitioning = true;
        IsDummyCompleted = false;
        IsPlayerCompleted = false;
        StartCoroutine(PlayTransitionAnimation(levelId, skipAnim));
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
            Player.transform.localPosition = Vector3.Lerp(playerPos, 
                CurrentLevel.PlayerStart.transform.localPosition, (Time.timeSinceLevelLoad - animStart) / 0.75F);

            if (HasPastLevel)
            {
                PlayerDummy.transform.localPosition = Vector3.Lerp(dummyPos,
                    PastLevel.PlayerStart.transform.localPosition, (Time.timeSinceLevelLoad - animStart) / 0.75F);
            }

            yield return null;
        }

        RewindEffect.enabled = false;
        ReloadLevel();
    }

    private IEnumerator PlayTransitionAnimation(int nextLevelId, bool skipAnim)
    {
        var animStart = Time.timeSinceLevelLoad;

        var returnBack = nextLevelId == _pastLevel;

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

        if (!skipAnim)
        {
            if (!returnBack)
            {
                while (Time.timeSinceLevelLoad - animStart < TransitionDuration)
                {
                    if (_pastLevel != -1)
                    {
                        pastLevel.transform.position =
                            new Vector3(0F, -4.5F - (Time.timeSinceLevelLoad - animStart) / TransitionDuration * 9F);
                        currentLevel.transform.position =
                            new Vector3(0F, 4.5F - (Time.timeSinceLevelLoad - animStart) / TransitionDuration * 9F);
                    }
                    else
                    {
                        currentLevel.transform.position =
                            new Vector3(0F, -(Time.timeSinceLevelLoad - animStart) / TransitionDuration * 4.5F);
                    }

                    futureLevel.transform.position =
                        new Vector3(0F, 13.5F - (Time.timeSinceLevelLoad - animStart) / TransitionDuration * 9F);

                    yield return null;
                }
            }
            else
            {
                while (Time.timeSinceLevelLoad - animStart < TransitionDuration)
                {
                    pastLevel.transform.position =
                        new Vector3(0F, 4.5F + (Time.timeSinceLevelLoad - animStart) / TransitionDuration * 9F);

                    if (_pastLevel != 0)
                    {
                        currentLevel.transform.position =
                            new Vector3(0F, -4.5F + (Time.timeSinceLevelLoad - animStart) / TransitionDuration * 9F);
                        futureLevel.transform.position =
                            new Vector3(0F, -13.5F + (Time.timeSinceLevelLoad - animStart) / TransitionDuration * 9F);
                    }
                    else
                    {
                        currentLevel.transform.position =
                            new Vector3(0F, -4.5F + (Time.timeSinceLevelLoad - animStart) / TransitionDuration * 4.5F);
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

        ReloadLevel();
        _isTransitioning = false;

        if (!skipAnim)
            HasProgressedAtLeastOnce = true;
    }
}