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
        }
        
        public override void Deactivate(bool fireEvent = true)
        {
            base.Deactivate(fireEvent);
            _doorCollider.enabled = false;
        }
    }
}