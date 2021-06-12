using System;
using System.Collections.Generic;
using LogicalElements;
using System.Collections.Generic;
using System.Linq;
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
        private ActivatableElementState[] _activatableStates;

        private PositionStateElement[] _positionStateElements;
        private PositionState[] _positionStates;
        
        public IEnumerable<ActivatableElement> GetAllActivatableElements => _levelActivatables;
        public IEnumerable<ListenerElement> GetLevelListeners => _levelActivatables.OfType<ListenerElement>().ToList();
        public IEnumerable<ActivatorElement> GetLevelActivators => _levelActivatables.OfType<ActivatorElement>().ToList();
        public TimelineController Timeline => _timelineController;
        
        private void Awake()
        {
            SaveLevelInitialState();
        }
        
        private void Start()
        {
            SaveLevelInitialState();
        }

        public void SaveLevelInitialState()
        {
           // _positionStateElements = GetComponentInChildren<PositionStateElement>();
            _levelActivatables = GetComponentsInChildren<ActivatableElement>();
            _activatableStates = new ActivatableElementState[_levelActivatables.Length];
            
           //_positionStates = new PositionState(_positionStateElements);

            for (int i = 0; i<_levelActivatables.Length; i++)
                _activatableStates[i] = _levelActivatables[i].GetState();
        }

        public void LoadLevelInitialState()
        {
            for (int i = 0; i<_levelActivatables.Length; i++) 
                _levelActivatables[i].SetState(_activatableStates[i]);
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