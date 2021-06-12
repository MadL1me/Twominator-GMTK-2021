using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Levels;
using LogicalElements;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public PlayerController Player;
    public PlaybackDummy PlayerDummy;
    public GameLevel[] Levels;

    public int StartingLevel;
    public float TransitionDuration = 0.75F;
    
    public bool IsDummyCompleted { get; set; }
    public bool IsPlayerCompleted { get; set; }

    private int _currentLevel = -1;
    private int _pastLevel = -1;
    private bool _isTransitioning;

    public bool HasPastLevel => _pastLevel != -1;
    public GameLevel CurrentLevel => _currentLevel < Levels.Length ? Levels[_currentLevel] : null;
    public GameLevel PastLevel => _pastLevel >= 0 ? Levels[_pastLevel] : null;

    private LevelActivatablesController _levelActivatablesController;
    
    private void Start()
    {
        TransitionToLevel(StartingLevel, true);
    }
    
    public void ReloadLevel()
    {
        if (_pastLevel >= 0)
            PastLevel.LoadLevelInitialState();
        
        CurrentLevel.LoadLevelInitialState();
        CurrentLevel.AssignObjectAndSpawnAtStart(Player.gameObject);

        if (_pastLevel != -1)
            PlayerDummy.RespawnDummy();
        
        Player.gameObject.SetActive(true);
        Player.GetComponent<Rigidbody2D>().simulated = true;

        IsDummyCompleted = false;
        IsPlayerCompleted = false;
        
        _levelActivatablesController?.UnSubscribeFromActivatorEvents();
        _levelActivatablesController = new LevelActivatablesController(PastLevel, CurrentLevel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) && HasPastLevel && !_isTransitioning)
            TransitionToPrevLevel();
        
        if (Input.GetKeyDown(KeyCode.R) && !_isTransitioning)
            ReloadLevel();
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

    private IEnumerator PlayTransitionAnimation(int nextLevelId, bool skipAnim)
    {
        var animStart = Time.timeSinceLevelLoad;

        var returnBack = nextLevelId == _pastLevel;

        var pastLevel = returnBack ? Levels[_currentLevel] :
            _pastLevel == -1 ? null : Levels[_pastLevel];
        var currentLevel = returnBack ? Levels[_pastLevel] :
            _currentLevel == -1 ? null : Levels[_currentLevel];
        var futureLevel = returnBack ? Levels[_pastLevel - 1] : Levels[nextLevelId];

        futureLevel.gameObject.SetActive(true);

        futureLevel.transform.position = returnBack ? new Vector3(0F, -12F) : new Vector3(0F, 12F);

        if (!skipAnim)
        {
            if (!returnBack)
            {
                while (Time.timeSinceLevelLoad - animStart < TransitionDuration)
                {
                    if (_pastLevel != -1)
                    {
                        pastLevel.transform.position =
                            new Vector3(0F, -4F - (Time.timeSinceLevelLoad - animStart) / TransitionDuration * 8F);
                        currentLevel.transform.position =
                            new Vector3(0F, 4F - (Time.timeSinceLevelLoad - animStart) / TransitionDuration * 8F);
                    }
                    else
                    {
                        currentLevel.transform.position =
                            new Vector3(0F, -(Time.timeSinceLevelLoad - animStart) / TransitionDuration * 4F);
                    }

                    futureLevel.transform.position =
                        new Vector3(0F, 12F - (Time.timeSinceLevelLoad - animStart) / TransitionDuration * 8F);

                    yield return null;
                }
            }
            else
            {
                while (Time.timeSinceLevelLoad - animStart < TransitionDuration)
                {
                    pastLevel.transform.position =
                        new Vector3(0F, 4F + (Time.timeSinceLevelLoad - animStart) / TransitionDuration * 8F);
                    currentLevel.transform.position =
                        new Vector3(0F, -4F + (Time.timeSinceLevelLoad - animStart) / TransitionDuration * 8F);
                    futureLevel.transform.position =
                        new Vector3(0F, -12F + (Time.timeSinceLevelLoad - animStart) / TransitionDuration * 8F);

                    yield return null;
                }
            }
        }

        if (!returnBack)
        {
            if (_pastLevel != -1)
                pastLevel.transform.position = new Vector3(0F, -12F);

            if (_currentLevel != -1)
            {
                currentLevel.transform.position = new Vector3(0F, -4F);
                futureLevel.transform.position = new Vector3(0F, 4F);
            }
            else
            {
                futureLevel.transform.position = new Vector3(0F, 0F);
            }
        }
        else
        {
            pastLevel.transform.position = new Vector3(0F, 12F);
            currentLevel.transform.position = new Vector3(0F, 4F);
            futureLevel.transform.position = new Vector3(0F, -4F);
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
    }
}