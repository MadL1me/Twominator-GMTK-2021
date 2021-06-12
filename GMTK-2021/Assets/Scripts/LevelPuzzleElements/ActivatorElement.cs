using System;
using UnityEngine;

namespace LogicalElements
{
    public class ActivatorElement : ActivatableElement
    {
        public ActivatableElement[] ConnectedActivatableElements => _connectedActivatableElements;
        
        [SerializeField] private ActivatableElement[] _connectedActivatableElements;

        public void SetConnectedElements(ActivatableElement[] elements)
        {
            _connectedActivatableElements = elements;
        }
        
        public override void Activate(bool fireEvent = true)
        {
            foreach (var connectedActivatableElement in ConnectedActivatableElements)
            {
                connectedActivatableElement.Activate();
            }
            
            base.Activate();
        }
        
        public override void Deactivate(bool fireEvent = true)
        {
            foreach (var connectedActivatableElement in ConnectedActivatableElements)
            {
                connectedActivatableElement.Deactivate();
            }
            
            base.Deactivate();
        }
    }
}