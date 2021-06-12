using System;
using Extensions;
using UnityEngine;

namespace LogicalElements
{
    public class PushableButton : ActivatorElement
    {
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

            if (_objectsOnButton <= 0)
            {
                if (IsActive)
                    Switch();
            }
        }

        public override void Activate(bool fireEvent = true)
        {
            base.Activate(fireEvent);
        }
        
        public override void Deactivate(bool fireEvent = true)
        {
            base.Deactivate(fireEvent);
        }
    }
}