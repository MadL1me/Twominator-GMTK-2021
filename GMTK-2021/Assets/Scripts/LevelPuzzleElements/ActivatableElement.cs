using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
            set => _colorEnum = value;
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
        [SerializeField] protected TilemapRenderer _activeTilemap;
        [SerializeField] protected TilemapRenderer _inactiveTilemap;
        [SerializeField] protected bool _isActive;
        
        public void SetState(ActivatableElementState state)
        {
            IsActive = state.IsActive;
                        
            if (IsActive)
                Activate(false);
            else
                Deactivate(false);
        }

        public ActivatableElementState GetState()
        {
            return new ActivatableElementState
            {
                IsActive = IsActive
            };
        }
        
        public virtual void Switch(bool fireEvent = true)
        {
            if (IsActive)
                Deactivate(false);
            else
                Activate(false);
            
            if (fireEvent)
                OnSwitch?.Invoke(ColorEnum);
        }
        
        public virtual void Activate(bool fireEvent = true)
        {
            print($"Activate at object: {gameObject.name}");
            
            IsActive = true;

            _activeTilemap.enabled = true;
            _inactiveTilemap.enabled = false;
            /*if (fireEvent)
                OnActivate?.Invoke(ColorEnum);*/
        }

        public virtual void Deactivate(bool fireEvent = true)
        {
            print($"Deactivate at object: {gameObject.name}");
            
            IsActive = false;
            
            _activeTilemap.enabled = false;
            _inactiveTilemap.enabled = true;
            /*if (fireEvent)
                OnDeactivate?.Invoke(ColorEnum);*/
        }
    }
}