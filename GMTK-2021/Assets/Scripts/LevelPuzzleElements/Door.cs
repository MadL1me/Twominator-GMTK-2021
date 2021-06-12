using System.Collections.Generic;
using Extensions;
using Levels;
using UnityEngine;

namespace LogicalElements
{
    public class Door : ListenerElement
    {
        [SerializeField] private Collider2D _doorCollider;
        
        public override void Activate(bool fireEvent = true)
        {
            base.Activate(fireEvent);
            _doorCollider.enabled = true;
            _sprite.SetAlpha(1f);
        }
        
        public override void Deactivate(bool fireEvent = true)
        {
            base.Deactivate(fireEvent);
            _doorCollider.enabled = false;
            _sprite.SetAlpha(0.3f);
        }
    }
}