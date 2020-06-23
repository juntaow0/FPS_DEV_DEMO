using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public AudioSource audiosource;

    public void useRadio()
    {
        audiosource.Play();
    }
}
