using System.Collections.Generic;
using Extensions;
using Levels;
using UnityEngine;

namespace LogicalElements
{
    public class Door : ListenerElement
    {
        [SerializeField] private Collider2D _doorCollider;
        
        public override void Activate()
        {
            base.Activate();
            _doorCollider.enabled = true;
            _sprite.SetAlpha(1f);
        }
        
        public override void Deactivate()
        {
            base.Deactivate();
            _doorCollider.enabled = false;
            _sprite.SetAlpha(0.3f);
        }
    }
}