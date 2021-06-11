using System;
using System.Collections.Generic;
using UnityEngine;

namespace LogicalElements
{
    public abstract class ActivatableElement : MonoBehaviour
    {
        public event Action OnActivated;
        public event Action OnDeactivated;
        
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
        
        [SerializeField] protected ColorEnum _colorEnum;
        [SerializeField] protected SpriteRenderer _sprite;
        [SerializeField] protected bool _isActive;
        
        public void SetState(ActivatableElementState state)
        {
            IsActive = state.IsActive;
        }

        public ActivatableElementState GetState()
        {
            return new ActivatableElementState
            {
                IsActive = IsActive
            };
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
            OnActivated?.Invoke();
        }

        public virtual void Deactivate()
        {
            IsActive = false;
            OnDeactivated?.Invoke();
        }
    }
}