using System.Collections.Generic;
using UnityEngine;

namespace LogicalElements
{
    public abstract class ActivatableElement : MonoBehaviour
    {
        public List<ActivatableElement> ConnectedActivatableElements { get; set; }
        
        public virtual void Activate()
        {
            foreach (var connectedActivatableElement in ConnectedActivatableElements)
            {
                connectedActivatableElement.Activate();
            }
        }

        public virtual void Deactivate()
        {
            foreach (var connectedActivatableElement in ConnectedActivatableElements)
            {
                connectedActivatableElement.Deactivate();
            }
        }
    }
}