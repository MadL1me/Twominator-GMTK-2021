using System;
using System.Collections.Generic;
using LogicalElements;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Levels
{
    public class GameLevel : MonoBehaviour
    {
        public bool IsCurrentLevelPlaying { get; set; } = true;

        public GameObject PlayerStart;

        private TimelineController _timelineController = new TimelineController();
        private ActivatableElement[] _levelActivatables;
        private ActivatableElementState[] _levelStates;
        
        public ActivatableElement[] GetAllActivatableElements() => _levelActivatables;
        
        private void Awake()
        {
            SaveLevelInitialState();
        }

        public TimelineController Timeline => _timelineController;
        
        private void Start()
        {
            SaveLevelInitialState();
        }

        public void SaveLevelInitialState()
        {
            _levelActivatables = GetComponentsInChildren<ActivatableElement>();
            _levelStates = new ActivatableElementState[_levelActivatables.Length];

            for (int i = 0; i<_levelActivatables.Length; i++)
                _levelStates[i] = _levelActivatables[i].GetState();
        }

        public void LoadLevelInitialState()
        {
            for (int i = 0; i<_levelActivatables.Length; i++)
                _levelActivatables[i].SetState(_levelStates[i]);
        }

        public void SavePlayerCommand(PlayerCommands command)
        {
            _timelineController.SaveCommand(command);
        }

        public void FinalizeSavedFrame()
        {
            _timelineController.AdvanceTick();
        }

        public void AssignObjectAndSpawnAtStart(GameObject gameObject)
        {
            gameObject.transform.SetParent(transform);
            gameObject.transform.localPosition = PlayerStart.transform.localPosition;
        }

        public void LoadLevelForPlaying()
        {
            LoadLevelInitialState();
            _timelineController.ReloadTimeline();
        }

        public void LoadLevelForRepeat()
        {
            LoadLevelInitialState();
            _timelineController.ClearTimeline();
        }
    }
}