using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellAudio : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private List<ParticleCollisionEvent> collisionEvents;
    public AudioClip shell;
    private int prevCount = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }
    private void OnParticleCollision(GameObject other)
    {
        int eventCount = particleSystem.GetCollisionEvents(other, collisionEvents);
        if (eventCount> prevCount)
        {
            prevCount = eventCount;
            AudioSource.PlayClipAtPoint(shell, collisionEvents[eventCount - 1].intersection);
        }
    }
}
