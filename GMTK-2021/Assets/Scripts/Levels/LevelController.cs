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

    private int _currentLevel = -1;
    private int _pastLevel = -1;
    private bool _isTransitioning;

    public GameLevel CurrentLevel => Levels[_currentLevel];
    public GameLevel PastLevel => Levels[_pastLevel];

    private void Start()
    {
        TransitionToLevel(StartingLevel, true);
    }
    
    public void ReloadLevel()
    {
        if (_pastLevel >= 0)
            PastLevel.LoadLevelInitialState();
        
        CurrentLevel.LoadLevelInitialState();
        SynchronizeLevelsActivatableElements();
    }

    public void SynchronizeLevelsActivatableElements()
    {
        var allActivators = new List<ActivatorElement>();
        var allActivatables = new List<ActivatableElement>();

        if (_pastLevel >= 0)
        {
            allActivators.AddRange(PastLevel.GetAllLevelActivators.ToList());
            allActivatables.AddRange(PastLevel.GetAllActivatableExceptActivators.ToList());
        }

        allActivators.AddRange(CurrentLevel.GetAllLevelActivators);
        allActivatables.AddRange(CurrentLevel.GetAllActivatableExceptActivators);

        var groupActivatorsByColorEnum = allActivators.GroupBy(activatable => activatable.ColorEnum);
        var groupActivatablesByColorEnum = allActivatables.GroupBy(activatable => activatable.ColorEnum);

        var colorToActivatable = new Dictionary<ColorEnum, List<ActivatableElement>>();

        foreach (var group in groupActivatablesByColorEnum)
        {
            if (!colorToActivatable.ContainsKey(group.Key))
            {
                colorToActivatable[group.Key] = new List<ActivatableElement>();
            }

            foreach (var activatable in group)
            {
                colorToActivatable[group.Key].Add(activatable);
            }
        }

        foreach (var colorGroup in groupActivatorsByColorEnum)
        {
            foreach (var activator in colorGroup)
            {
                activator.SetConnectedElements(colorToActivatable[colorGroup.Key].ToArray());
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1) && !_isTransitioning)
            TransitionToLevel(_currentLevel + 1);

        if (Input.GetKeyDown(KeyCode.F2) && !_isTransitioning)
            TransitionToLevel(_currentLevel - 1);
    }

    public void TransitionToNextLevel()
    {
        Player.ReassignToLevel(Levels[_currentLevel + 1]);
        TransitionToLevel(_currentLevel + 1);
    }

    public void TransitionToLevel(int levelId, bool skipAnim = false)
    {
        _isTransitioning = true;
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

        CurrentLevel.AssignObjectAndSpawnAtStart(Player.gameObject);

        if (_pastLevel != -1)
            PlayerDummy.RespawnDummy();

        ReloadLevel();
        Player.gameObject.SetActive(true);
        _isTransitioning = false;
    }
}