using System;
using Extensions;
using UnityEngine;

namespace LogicalElements
{
    [RequireComponent(typeof(Collider2D))]
    public class SwitchButton : ActivatorElement
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            print("TRIGGER");
            Switch();
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