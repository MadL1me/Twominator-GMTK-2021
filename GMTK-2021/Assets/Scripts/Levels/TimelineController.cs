using System.Collections.Generic;
using System.Linq;
using Player;

namespace Levels
{
    public class TimelineController
    {
        public Dictionary<float, List<PlayerCommand>> TimeToPlayerCommandExecuted { get; private set; }
            = new Dictionary<float, List<PlayerCommand>>(); 
        
        public float CurrentTimestep { get; private set; }
        
        private Queue<(float, List<PlayerCommand>)> _repeatCommandsQueue;

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
            {
                var executingCommands = _repeatCommandsQueue.Dequeue();
                foreach (var command in executingCommands.Item2)
                {
                    command.Execute();
                }
            }
        }

        public void SaveCommand(PlayerCommand command)
        {
            if (!TimeToPlayerCommandExecuted.ContainsKey(CurrentTimestep))
                TimeToPlayerCommandExecuted.Add(CurrentTimestep, new List<PlayerCommand>());
            
            TimeToPlayerCommandExecuted[CurrentTimestep].Add(command);
        }
        
        public void ClearTimeline()
        {
            TimeToPlayerCommandExecuted.Clear();
        }
    }
}