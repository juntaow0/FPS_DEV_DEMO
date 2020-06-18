﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _gravity = -9.81f;
    [SerializeField]
    private float _jumpHeight = 2f;
    [SerializeField]
    private float _startSmoothTime = 0.22f;
    [SerializeField]
    private float _stopSmoothTime = 0.16f;
    [SerializeField]
    private float _airFriction = 0.15f;

    private Vector3 _velocity;
    private Vector3 _refVelocityLand;
    private float _jumpVelocity;

    public bool isMoving { get; private set; }

    private CharacterController _cc;
    private PlayerInputClass _pi;

    private Vector3 prevDir;

    Vector2 movementInput;
    private void Awake()
    {
        _pi = new PlayerInputClass();
        _pi.Player.Move.performed += context => movementInput = context.ReadValue<Vector2>();
        _cc = GetComponent<CharacterController>();
        if (_cc == null)
        {
            Debug.LogError("No Character Controller attached!");
        }
        _jumpVelocity = _gravity;
        isMoving = false;
    }

    private void Update()
    {
        movement();
    }

    void jump()
    {
        _jumpVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
    }

    void movement()
    {
        float x = movementInput.x;
        float z = movementInput.y;
        Vector3 dir = new Vector3(x, 0f, z);
        if (dir.sqrMagnitude == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }


        dir = transform.TransformDirection(dir).normalized;
        _velocity.y = 0;
        
        if (_cc.isGrounded)
        {
            _jumpVelocity = _gravity;
            if (_pi.Player.Jump.triggered)
            {
                jump();
            }
            if (dir.sqrMagnitude == 0 && _velocity.sqrMagnitude!=0)
            {
                _velocity = Vector3.SmoothDamp(_velocity, Vector3.zero, ref _refVelocityLand, _stopSmoothTime);
            }
            else
            {
                Vector3 targetVelocity = dir * _speed;
                _velocity = Vector3.SmoothDamp(_velocity, targetVelocity, ref _refVelocityLand, _startSmoothTime);
                prevDir = dir;
            }
        }
        else
        {
            isMoving = false;
            _jumpVelocity += _gravity * Time.deltaTime;

            if (x != 0)
            {
                _velocity = _velocity.magnitude * transform.forward;
            }
            
            _velocity -= (_velocity * _airFriction)*Time.deltaTime;
        }
        
        _velocity.y = _jumpVelocity;
        _cc.Move(_velocity * Time.deltaTime);
    }

    private void OnEnable()
    {
        _pi.Player.Move.Enable();
        _pi.Player.Jump.Enable();
    }

    private void OnDisable()
    {
        _pi.Player.Move.Disable();
        _pi.Player.Jump.Disable();
    }
}
