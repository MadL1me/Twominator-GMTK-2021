using Extensions;
using Levels;
using UnityEngine;

namespace LogicalElements
{
    public class Door : ActivatableElement
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
            base.Activate();
            _doorCollider.enabled = false;
            _sprite.SetAlpha(0.3f);
        }
    }
}