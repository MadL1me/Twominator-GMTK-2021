using System;
using System.Collections.Generic;
using UnityEngine;

namespace LogicalElements
{
    public abstract class ActivatableElement : MonoBehaviour
    {
        public event Action<ColorEnum> OnActivate;
        public event Action<ColorEnum> OnDeactivate;
        public event Action<ColorEnum> OnSwitch;
        
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
                        
            if (IsActive)
                Activate();
            else
                Deactivate();
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
            print($"SWITCH IN {gameObject.name}");
            OnSwitch?.Invoke(ColorEnum);

            if (IsActive)
                Deactivate();
            else
                Activate();
        }
        
        public virtual void Activate()
        {
            IsActive = true;
            //OnActivate?.Invoke(ColorEnum);
        }

        public virtual void Deactivate()
        {
            IsActive = false;
            //OnDeactivate?.Invoke(ColorEnum);
        }
    }
}