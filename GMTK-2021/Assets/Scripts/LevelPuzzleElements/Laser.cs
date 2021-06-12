using System;
using Extensions;
using UnityEngine;

namespace LogicalElements
{
    public class Laser : ListenerElement
    {
        [SerializeField] private Transform _laserInitiator;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            print("LASER TRIGGER OMFG");
            
            if (IsActive && other.gameObject.tag.Equals("Player"))
            {
                other.gameObject.GetComponent<PlayerController>().Die();
            }
        }

        public void RayCast()
        {
            var endPoint = Physics2D.Raycast(_laserInitiator.transform.position, _laserInitiator.up);
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