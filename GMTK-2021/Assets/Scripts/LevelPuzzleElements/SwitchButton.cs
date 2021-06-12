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

        public override void Activate()
        {
            base.Activate();
            _sprite.SetAlpha(1);
        }
       
        public override void Deactivate()
        {
            base.Deactivate();
            _sprite.SetAlpha(0.3f);
        }
    }
}