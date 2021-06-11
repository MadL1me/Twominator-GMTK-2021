using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;

namespace Levels
{
    public class TimelineController
    {
        public int CurrentTick;
        private Queue<PlayerCommand> _commands = new Queue<PlayerCommand>();

        public void ReloadTimeline()
        {
            CurrentTick = 0;
        }

        public IEnumerable<PlayerCommand> GetPlaybackForCurrentTickAndAdvance()
        {
            while (_commands.Count > 0)
            {
                var cmd = _commands.Peek();
                
                if (cmd.Tick != CurrentTick)
                    break;

                yield return _commands.Dequeue();
            }

            CurrentTick++;
        }

        public void SaveCommand(PlayerCommands command)
        {
            _commands.Enqueue(new PlayerCommand { Tick = CurrentTick, Type = command });
        }

        public void AdvanceTick()
        {
            CurrentTick++;
        }
        
        public void ClearTimeline()
        {
            _commands.Clear();
            CurrentTick = 0;
        }
    }
}