using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip walk;
    public AudioClip jump_start;
    public AudioClip jump_end;
    public AudioSource audioSource;
    public AudioSource jumpSource;
    private PlayerMovement playerMovement;
    private CharacterController cc;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (playerMovement.isMoving)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = walk;
                audioSource.Play();
            }
        }
        else if (cc.isGrounded && !playerMovement.isMoving)
        {
            audioSource.Stop();
        }
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(jump_start);
        }
        if (!playerMovement.wasGrounded&&cc.isGrounded)
        {
            jumpSource.PlayOneShot(jump_end);
        }
    }
}
