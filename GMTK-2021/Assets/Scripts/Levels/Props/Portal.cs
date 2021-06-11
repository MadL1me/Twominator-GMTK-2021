using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public LevelController LevelController;
    public bool IsExitPortal;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || IsExitPortal)
            return;
        
        other.gameObject.SetActive(false);
        
        LevelController.TransitionToNextLevel();
    }
}
