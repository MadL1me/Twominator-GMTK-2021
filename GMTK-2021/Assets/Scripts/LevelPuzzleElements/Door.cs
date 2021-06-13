using System.Collections.Generic;
using Extensions;
using Levels;
using UnityEngine;

namespace LogicalElements
{
    public class Door : ListenerElement
    {
        [SerializeField] private Collider2D _doorCollider;
        
        public override void Activate(bool playSound = true)
        {
            base.Activate(playSound);
            _doorCollider.enabled = true;
        }
        
        public override void Deactivate(bool playSound = true)
        {
            base.Deactivate(playSound);
            _doorCollider.enabled = false;
        }
    }
}