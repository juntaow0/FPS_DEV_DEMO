using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAlarm : MonoBehaviour
{
    public void press()
    {
        AudioManager.instance.Play("scream");
    }
}
