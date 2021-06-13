using System;
using Extensions;
using Unity.VisualScripting;
using UnityEngine;

namespace LogicalElements
{
    [RequireComponent(typeof(Collider2D))]
    public class SwitchButton : ActivatorElement
    {
        public bool PersistentButton = true;

        protected bool _isTouched;
        protected PlayerController _touching;

        public bool IsPressed { get; set; }
        
        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            _isTouched = true;
            _touching = other.GetComponent<PlayerController>();
        }

        protected void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (_touching.GetComponent<Rigidbody2D>().simulated)
                _isTouched = false;
        }

        protected virtual void Update()
        {
            if (PersistentButton)
            {
                if (!_isTouched)
                    return;

                if (!IsActive)
                    return;

                if (_touching.JustPressedUse)
                    Switch();
            }
            else if (!PersistentButton)
            {
                if (!_isTouched)
                {
                    if (IsActive)
                        Switch();

                    return;
                }
                
                if (_touching.JustPressedUse || _touching.JustUnpressedUse)
                    Switch();
            }
        }
    }
}