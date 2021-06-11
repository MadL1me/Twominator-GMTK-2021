using System;
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

        public TimelineController Timeline => _timelineController;
        
        private void Start()
        {
            SaveGameLevelStartingState();
        }

        private void SaveGameLevelStartingState()
        {
            // saving starting state
        }

        public void RestartLevel()
        {
            _timelineController.ReloadTimeline();
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

        public void ActivateLevel()
        {
            
        }
    }
}