using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class RemainingTimelineTimeUi : MonoBehaviour
    {
        [SerializeField] private Slider _timeSlider;
        [SerializeField] private Canvas _canvas;

        public void Enable()
        {
            _canvas.enabled = true;
        }

        public void Disable()
        {
            _canvas.enabled = false;
        }
        
        public void InitSliderValues(float totalTime)
        {
            _timeSlider.maxValue = totalTime;
            _timeSlider.minValue = 0;
        }
        
        public void SetTimelineValue(float time)
        {
            if (!_canvas.enabled)
                return;
            
            print(time);
            _timeSlider.value = time;
        }
    }
}