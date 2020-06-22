using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAudio : MonoBehaviour
{
    public AudioClip weapon_drop;
    public AudioClip weapon_fire;
    public AudioClip weapon_reload;
    public AudioClip weapon_switch;
    public AudioSource audiosource;
    public void fireSound()
    {
        audiosource.PlayOneShot(weapon_fire);
    }

    public void reloadSound()
    {
        audiosource.PlayOneShot(weapon_reload);
    }

    public void switchSound() 
    {
        audiosource.PlayOneShot(weapon_switch);
    }

    private void OnTriggerEnter(Collider other)
    {
        audiosource.PlayOneShot(weapon_drop);
    }
}
