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

    private void Start()
    {
        var controller = GetComponent<PlayerController>();
        controller.SetupDummy(Player);

        _controller = controller;
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
        var rb = GetComponent<Rigidbody2D>();
        rb.simulated = true;
        rb.velocity = Vector2.zero;
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
        if (!_spawned)
            return;
        
        foreach (var cmd in LevelController.PastLevel.Timeline.GetPlaybackForCurrentTickAndAdvance())
            _controller.PlayCommand(cmd);
        
        if (LevelController.PastLevel.Timeline.HasEndedPlayback && 
            LevelController.IsPlayerCompleted && !LevelController.IsDummyCompleted)
            LevelController.ReloadLevel();
    }
}
