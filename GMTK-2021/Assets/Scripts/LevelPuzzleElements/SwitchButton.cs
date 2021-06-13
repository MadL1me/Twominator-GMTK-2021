using System;
using Extensions;
using UnityEngine;

namespace LogicalElements
{
    [RequireComponent(typeof(Collider2D))]
    public class SwitchButton : ActivatorElement
    {
        private bool _isTouched;
        private PlayerController _touching;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            _isTouched = true;
            _touching = other.GetComponent<PlayerController>();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            _isTouched = false;
        }

        private void Update()
        {
            if (!_isTouched || !_touching.JustPressedUse)
                return;
            
            print("TRIGGER");
            Switch();
        }
    }
}