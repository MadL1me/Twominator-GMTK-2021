using System;
using Extensions;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace LogicalElements
{
    public class Laser : ListenerElement
    {
        [SerializeField] private Transform _laserInitiator;
        [SerializeField] private Transform _laserSprite;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            print("LASER TRIGGER OMFG");
            
            if (IsActive && other.gameObject.tag.Equals("Player"))
            {
                other.gameObject.GetComponent<PlayerController>().Die();
            }
        }

        public void FixedUpdate()
        {
            if (_isActive)
            {
                _laserSprite.gameObject.SetActive(true);
                RayCast();
            }
            else
            {
                _laserSprite.gameObject.SetActive(false);
            }
        }

        public void RayCast()
        {
            var endPoint = Physics2D.Raycast(_laserInitiator.transform.position, 
                _laserInitiator.up, 50);
            
            print(endPoint.point);
            
            if (endPoint)
            {
                CalculateLaserSpriteProps(endPoint);
            }
            else
            {
                SetLaserSpriteProps();
            }
            
            if (IsActive && endPoint.collider != null && endPoint.collider.gameObject.tag.Equals("Player"))
                endPoint.collider.gameObject.GetComponent<PlayerController>().Die();
        }

        private void CalculateLaserSpriteProps(RaycastHit2D raycastHit2D)
        {
            var localPosition = transform.InverseTransformPoint(raycastHit2D.point); // transform.InverseTransformPoint(raycastHit2D.transform.position);
            var newScale = localPosition;
            newScale = new Vector3(localPosition.x, 1.5f, 1);
            
            _laserSprite.localPosition = localPosition / 2;
            _laserSprite.localScale = newScale;
        }

        private void SetLaserSpriteProps()
        {
            _laserSprite.localPosition = new Vector3(25, 0);
            _laserSprite.localScale = new Vector3(50, 1.5f);
        }
    }
}