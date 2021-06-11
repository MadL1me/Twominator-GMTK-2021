using System;
using System.Collections.Generic;
using UnityEngine;

namespace LogicalElements
{
    public abstract class ActivatableElement : MonoBehaviour
    {
        public ColorEnum ColorEnum 
        {
            get => _colorEnum;
            set
            {
                _colorEnum = value;
                _sprite.color = value.Color;
            } 
        }
        
        public bool IsActive
        {
            get => _isActive;
            private set
            {
                _isActive = value;
            } 
        }
        
        public List<ActivatableElement> ConnectedActivatableElements => _activatableElements;

        [SerializeField] protected List<ActivatableElement> _activatableElements;
        [SerializeField] protected ColorEnum _colorEnum;
        [SerializeField] protected SpriteRenderer _sprite;
        [SerializeField] protected bool _isActive;
        
        protected virtual void Awake()
        {
            
        }

        public virtual void Switch()
        {
            if (IsActive)
                Deactivate();
            else
                Activate();
        }
        
        public virtual void Activate()
        {
            IsActive = true;
            
            foreach (var connectedActivatableElement in ConnectedActivatableElements)
            {
                connectedActivatableElement.Activate();
            }
        }

        public virtual void Deactivate()
        {
            IsActive = false;
            
            foreach (var connectedActivatableElement in ConnectedActivatableElements)
            {
                connectedActivatableElement.Deactivate();
            }
        }
    }
}