using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using Levels;
using Player;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{ 
    [SerializeField] private GameLevel _levelAttachedToPlayer;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _downRaycastDistance = 1;
    
    private Rigidbody2D _rigidbody;
    private bool[] _captureCmds;

    public bool IsControllable { get; set; } = true;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _captureCmds = new bool[Enum.GetValues(typeof(PlayerCommands)).Length];
    }

    public void ReassignToLevel(GameLevel level)
    {
        _levelAttachedToPlayer = level;
    }

    private void Update()
    {
        if (!IsControllable)
            return;

        if (Input.GetKey(KeyCode.A))
            MoveLeft();
        else if (Input.GetKey(KeyCode.D))
            MoveRight();
        
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }

    private void ResetCapture()
    {
        for (var i = 0; i < _captureCmds.Length; i++)
            _captureCmds[i] = false;
    }

    private void FixedUpdate()
    {
        if (!IsControllable)
            return;
        
        for (var i = 0; i < _captureCmds.Length; i++)
        {
            if (_captureCmds[i])
                _levelAttachedToPlayer.SavePlayerCommand((PlayerCommands) i);
        }

        _levelAttachedToPlayer.FinalizeSavedFrame();
        
        ResetCapture();
    }

    private bool OnGround()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, _downRaycastDistance);
    }

    public void MoveLeft()
    {
        _rigidbody.velocity = new Vector2((_playerSpeed * Vector2.left).x, _rigidbody.velocity.y);
        _captureCmds[(int) PlayerCommands.MoveLeft] = true;
        _captureCmds[(int) PlayerCommands.MoveRight] = false;
    }

    public void MoveRight()
    {
        _rigidbody.velocity = new Vector2((_playerSpeed * Vector2.right).x, _rigidbody.velocity.y);
        _captureCmds[(int) PlayerCommands.MoveRight] = true;
        _captureCmds[(int) PlayerCommands.MoveLeft] = false;
    }

    public void Jump()
    {
        _rigidbody.AddForce(Vector2.up * _jumpForce);
        _captureCmds[(int) PlayerCommands.Jump] = true;
    }

    public void PlayCommand(PlayerCommand command)
    {
        switch (command.Type)
        {
            case PlayerCommands.MoveLeft: 
                MoveLeft();
                break;
            case PlayerCommands.MoveRight: 
                MoveRight();
                break;
            case PlayerCommands.Jump:
                Jump();
                break;
        }
    }

    public void SetupDummy(PlayerController real)
    {
        _jumpForce = real._jumpForce;
        _playerSpeed = real._playerSpeed;
        _downRaycastDistance = real._downRaycastDistance;

        IsControllable = false;
    }
}
