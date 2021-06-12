using Ui;
using UnityEngine;

namespace Levels
{
    public class LevelUiController : MonoBehaviour
    { 
        [SerializeField] private RemainingTimelineTimeUi _timelineTimeUi;

        public void InitLevelUi()
        {
           
        }

        public void Enable()
        {
            _timelineTimeUi.Enable();
        }

        public void Disable()
        {
            _timelineTimeUi.Disable();
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