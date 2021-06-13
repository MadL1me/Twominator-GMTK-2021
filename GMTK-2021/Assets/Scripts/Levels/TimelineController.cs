using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;

namespace Levels
{
    public class TimelineController
    {
        public int CurrentTick;
        public int LastValidTick;
        
        public bool IsTimelocked { get; set; }
        
        private List<PlayerCommand> _commands = new List<PlayerCommand>();
        private int _curId;

        public bool HasEndedPlayback => CurrentTick > LastValidTick + 30;

        public void ReloadTimeline()
        {
            CurrentTick = 0;
            _curId = 0;
        }

        public IEnumerable<PlayerCommand> GetPlaybackForCurrentTickAndAdvance()
        {
            while (_curId < _commands.Count)
            {
                if (_commands[_curId].Tick != CurrentTick)
                    break;

                yield return _commands[_curId++];
            }

            CurrentTick++;
        }

        public void SaveCommand(PlayerCommands command)
        {
            _commands.Add(new PlayerCommand { Tick = CurrentTick, Type = command });
            LastValidTick = CurrentTick;
        }

        public void AdvanceTick()
        {
            if (IsTimelocked)
                return;
            
            CurrentTick++;
        }
        
        public void ClearTimeline()
        {
            _commands.Clear();
            CurrentTick = 0;
            _curId = 0;
        }
    }
}