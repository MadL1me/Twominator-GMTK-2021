using System;
using Extensions;
using UnityEngine;

namespace LogicalElements
{
    [RequireComponent(typeof(Collider2D))]
    public class SwitchButton : ActivatorElement
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Switch();
        }
    }
}