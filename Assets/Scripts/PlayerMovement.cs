using System.Collections;
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

    private float _jumpVelocity;
    private CharacterController _cc;
    private PlayerInputClass _pi;

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
        if (_cc.isGrounded)
        {
            _jumpVelocity = _gravity;
            if (_pi.Player.Jump.triggered)
            {
                jump();
            }
        }
        else
        {
            _jumpVelocity += _gravity * Time.deltaTime;
        }
        Vector3 dir = new Vector3(x, 0f, z);
        Vector3 velocity = dir * _speed;
        velocity = transform.TransformDirection(velocity);
        velocity.y = _jumpVelocity;
        _cc.Move(velocity * Time.deltaTime);
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
