using System;
using Player;
using UnityEngine;

namespace Levels
{
    public class GameLevel : MonoBehaviour
    {
        public bool IsCurrentLevelPlaying { get; set; } = true;

        private TimelineController _timelineController = new TimelineController();
        
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

        public void SavePlayerCommand(PlayerCommand command)
        {
            //print("SAVE PLAYER COMAND");
            
            if (IsCurrentLevelPlaying)
                command.Execute();

            _timelineController.SaveCommand(command);
        }

        public void UpdateGameLevel(float deltaTime)
        {
            _timelineController.UpdateState(deltaTime);
        }

        public void ActivateLevel()
        {
            
        }
    }
}