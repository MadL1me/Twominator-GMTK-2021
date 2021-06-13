using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaybackDummy : MonoBehaviour
{
    public PlayerController Player;
    public LevelController LevelController;

    private PlayerController _controller;
    private bool _spawned;
    private Rigidbody2D _rigidbody;
    
    public bool IsTimeLocked { get; private set; }

    private Vector2 _savedVelocity;
    private Vector2 _savedPosition;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        
        var controller = GetComponent<PlayerController>();
        controller.SetupDummy(Player);

        _controller = controller;
    }

    public void Timelock()
    {
        if (IsTimeLocked)
        {
            print("Locked");
            IsTimeLocked = false;
            _rigidbody.velocity = _savedVelocity;
            //transform.localPosition = _savedPosition;
            _rigidbody.simulated = true;
        }
        else
        {
            print("Unlocked");
            IsTimeLocked = true;
            _savedVelocity = _rigidbody.velocity;
            //_savedPosition = transform.localPosition;
            _rigidbody.simulated = false;
        }
    }
    
    public void RespawnDummy()
    {
        if (!LevelController.HasPastLevel)
        {
            gameObject.SetActive(false);
            _spawned = false;
            return;
        }
        
        gameObject.SetActive(true);
        GetComponent<Rigidbody2D>().simulated = true;
        LevelController.PastLevel.AssignObjectAndSpawnAtStart(gameObject);
        RewindDummy();
        _spawned = true;
    }

    public void RewindDummy()
    {
        LevelController.PastLevel.Timeline.ReloadTimeline();
    }

    private void FixedUpdate()
    {
        if (!_spawned || IsTimeLocked)
            return;
        
        foreach (var cmd in LevelController.PastLevel.Timeline.GetPlaybackForCurrentTickAndAdvance())
            _controller.PlayCommand(cmd);
        
        if (LevelController.PastLevel.Timeline.HasEndedPlayback && 
            LevelController.IsPlayerCompleted && !LevelController.IsDummyCompleted)
            LevelController.ReloadLevel();
    }
}
