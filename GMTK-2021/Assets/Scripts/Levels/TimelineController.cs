using System.Collections.Generic;
using System.Linq;
using Player;

namespace Levels
{
    public class TimelineController
    {
        public Dictionary<float, PlayerCommand> TimeToPlayerCommandExecuted { get; private set; }
            = new Dictionary<float, PlayerCommand>(); 
        
        public float CurrentTimestep { get; private set; }
        
        private Queue<(float, PlayerCommand)> _repeatCommandsQueue;

        public void ReloadTimeline()
        {
            foreach (var playerCommandInTime in TimeToPlayerCommandExecuted)
                _repeatCommandsQueue.Enqueue((playerCommandInTime.Key, playerCommandInTime.Value));
            
            CurrentTimestep = 0;
        }
        
        public void UpdateState(float timeAdd, bool executePlayerCommands = false)
        {
            CurrentTimestep += timeAdd;

            if (!executePlayerCommands)
                return;

            if (_repeatCommandsQueue.Peek().Item1 <= CurrentTimestep)
                _repeatCommandsQueue.Dequeue();
        }

        public void SaveCommand(PlayerCommand command)
        {
            TimeToPlayerCommandExecuted.Add(CurrentTimestep, command);
        }
        
        public void ClearTimeline()
        {
            TimeToPlayerCommandExecuted .Clear();
        }
    }
}