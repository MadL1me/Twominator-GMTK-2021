using Ui;
using UnityEngine;

namespace Levels
{
    public class LevelUiController : MonoBehaviour
    {
        [SerializeField] private Canvas _uiCanvas;
        [SerializeField] private RemainingTimelineTimeUi _timelineTimeUi;

        public void InitLevelUi()
        {
           
        }

        public void Enable()
        {
            _uiCanvas.enabled = true;
        }

        public void Disable()
        {
            _uiCanvas.enabled = false;
        }
        
        public void OnNextLevelStart(GameLevel lastLevel, GameLevel nextLevel)
        {
            _timelineTimeUi.InitSliderValues(lastLevel.Timeline.LastValidTick);
        }

        public void UpdateUi(float timelineTime) 
        {
            _timelineTimeUi.SetTimelineValue(timelineTime);
        }
    }
}