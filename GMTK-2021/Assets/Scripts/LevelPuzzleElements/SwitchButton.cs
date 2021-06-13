﻿using System;
using Extensions;
using UnityEngine;

namespace LogicalElements
{
    [RequireComponent(typeof(Collider2D))]
    public class SwitchButton : ActivatorElement
    {
        protected bool _isActivated;
        protected bool _isTouched;
        protected PlayerController _touching;
        
        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            _isTouched = true;
            _touching = other.GetComponent<PlayerController>();
        }

        protected void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            _isTouched = false;
        }

        protected virtual void Update()
        {
            if (!_isTouched)
                return;

            if (_isActivated)
                return;
            
            if (_touching.JustPressedUse)
                _isActivated = true;
            
            Switch();
        }
    }
}