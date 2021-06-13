using System;
using Extensions;
using UnityEngine;

namespace LogicalElements
{
    public class PushableButton : ActivatorElement
    {
        public bool PersistentButton = true;
        
        private int _objectsOnButton = 0;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            print("fucking trigger");
            
            if (other.gameObject.layer.Equals(7))
            {
                print("fucking activate");
                _objectsOnButton++;
                if (!IsActive)
                    Switch();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer.Equals(7))
            {
                _objectsOnButton--;
            }

            if (_objectsOnButton <= 0 && !PersistentButton)
            {
                if (IsActive)
                    Switch();
            }
        }

        public override void Activate(bool playSound = true)
        {
            base.Activate(playSound);
        }
        
        public override void Deactivate(bool playSound = true)
        {
            base.Deactivate(playSound);
        }
    }
}