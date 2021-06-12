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

        if (other.GetComponent<PlaybackDummy>() == null)
            LevelController.IsPlayerCompleted = true;
        else
            LevelController.IsDummyCompleted = true;

        other.GetComponent<Rigidbody2D>().simulated = false;

        if (LevelController.HasPastLevel &&
            (!LevelController.IsPlayerCompleted || !LevelController.IsDummyCompleted))
            return;
        
        LevelController.Player.gameObject.SetActive(false);
        LevelController.PlayerDummy.gameObject.SetActive(false);

        LevelController.TransitionToNextLevel();
    }
}
