using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private Animator _animator;
    // Start is called before the first frame update
    private void Awake()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool("isWalking", _playerMovement.isMoving);

    }
    private void OnEnable()
    {
        _animator.SetBool("isWalking", false);
    }
    private void OnDisable()
    {
        _animator.SetBool("isWalking", false);
    }
}
