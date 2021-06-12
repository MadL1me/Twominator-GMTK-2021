using System;
using Extensions;
using UnityEngine;

namespace LogicalElements
{
    public class Laser : ListenerElement
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            print("LASER TRIGGER OMFG");
            
            if (IsActive && other.gameObject.tag.Equals("Player"))
            {
                other.gameObject.GetComponent<PlayerController>().Die();
            }
        }
        
        public override void Activate(bool fireEvent = true)
        {
            base.Activate(fireEvent);
            _sprite.SetAlpha(1f);
        }
        
        public override void Deactivate(bool fireEvent = true)
        {
            base.Deactivate(fireEvent);
            _sprite.SetAlpha(0.3f);
        }
    }
}