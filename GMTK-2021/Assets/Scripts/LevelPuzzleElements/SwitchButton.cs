using System;
using Extensions;
using UnityEngine;

namespace LogicalElements
{
    [RequireComponent(typeof(Collider2D))]
    public class SwitchButton : ActivatorElement
    {
        public bool PersistentButton = true;
        
        protected bool _isTouched;
        protected PlayerController _touching;
        
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

            _isTouched = false;
        }

        protected virtual void Update()
        {
            if (PersistentButton && _isTouched)
            {
                if (!IsActive)
                    return;

                if (_touching.JustPressedUse)
                    Switch();
            }
            else if (_touching != null)
            {
                if (_touching.JustPressedUse || _touching.JustUnpressedUse || (IsActive && !_isTouched))
                    Switch();
            }
        }
    }
}