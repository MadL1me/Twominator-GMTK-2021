using System;
using System.Collections.Generic;
using LogicalElements;
using Player;
using UnityEngine;

namespace Levels
{
    public class GameLevel : MonoBehaviour
    {
        public bool IsCurrentLevelPlaying { get; set; } = true;

        private TimelineController _timelineController = new TimelineController();
        private PlayerController _playerController;
        private ActivatableElement[] _levelActivatables;
        private ActivatableElementState[] _levelStates;
        private Vector3 _playerStartPosition;
        
        public ActivatableElement[] GetAllActivatableElements() => _levelActivatables;
        
        private void Awake()
        {
            _playerController = GetComponentInChildren<PlayerController>();
            SaveLevelInitialState();
        }
        
        private void Start()
        {
            SaveLevelInitialState();
        }

        public void SaveLevelInitialState()
        {
            _playerStartPosition = _playerController.transform.position;
            _levelActivatables = GetComponentsInChildren<ActivatableElement>();
            _levelStates = new ActivatableElementState[_levelActivatables.Length];

            for (int i = 0; i<_levelActivatables.Length; i++)
                _levelStates[i] = _levelActivatables[i].GetState();
        }

        public void LoadLevelInitialState()
        {
            _playerController.transform.position = _playerStartPosition;

            for (int i = 0; i<_levelActivatables.Length; i++)
                _levelActivatables[i].SetState(_levelStates[i]);
        }

        public void SavePlayerCommand(PlayerCommand command)
        {
            if (IsCurrentLevelPlaying)
                command.Execute();

            _timelineController.SaveCommand(command);
        }

        public void UpdateGameLevel(float deltaTime)
        {
            _timelineController.UpdateState(deltaTime);
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