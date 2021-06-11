using System.Collections.Generic;
using System.Linq;
using LogicalElements;
using Unity.VisualScripting;
using UnityEngine;

namespace Levels
{
    public class TwoLevelsJoinedController : MonoBehaviour
    {
        private List<GameLevel> _levels; 
        
        private GameLevel _pastLevel;
        private GameLevel _currentFutureLevel;

        private int _levelIndex = 0;
        
        public void LoadNextLevel()
        {
            _currentFutureLevel.IsCurrentLevelPlaying = false;

            _pastLevel = _levels[_levelIndex];
            _currentFutureLevel = _levels[_levelIndex + 1];
            _currentFutureLevel.IsCurrentLevelPlaying = true;
                
            _levelIndex++;
            
            ReloadLevel();
        }

        public void LoadPreviousLevel()
        {
            _currentFutureLevel.IsCurrentLevelPlaying = false;

            _pastLevel = _levels[_levelIndex];
            _currentFutureLevel = _levels[_levelIndex + 1];
            _currentFutureLevel.IsCurrentLevelPlaying = true;
                
            _levelIndex++;
            
            ReloadLevel();
        }

        public void ReloadLevel()
        {
            _pastLevel.LoadLevelInitialState();
            _currentFutureLevel.LoadLevelInitialState();
        }

        public void SynchronizeLevelsActivatableElements()
        {
            var allActivators = _pastLevel.GetAllActivatableElements().OfType<ActivatorElements>();
            var groupByColorEnum = allActivators.GroupBy(activatable => activatable.ColorEnum);
        }
    }
}