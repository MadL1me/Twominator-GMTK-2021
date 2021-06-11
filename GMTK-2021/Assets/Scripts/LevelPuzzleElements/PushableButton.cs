using System;
using Extensions;
using UnityEngine;

namespace LogicalElements
{
    public class PushableButton : ActivatorElements
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Activate();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Deactivate();
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