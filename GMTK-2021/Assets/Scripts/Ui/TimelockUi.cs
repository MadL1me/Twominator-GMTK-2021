using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class TimelockUi : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Text _text;

        public void TimelockOn()
        {
            _image.enabled = true;
            _text.enabled = true;
        }

        public void TimelockOff()
        {
            _image.enabled = false;
            _text.enabled = false;
        }
    }
}