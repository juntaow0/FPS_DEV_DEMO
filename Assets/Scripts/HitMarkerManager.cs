using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMarkerManager:MonoBehaviour
{
    public static HitMarkerManager instance { get; private set; }
    public GameObject[] hitMarkers;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void selectHitMarker(RaycastHit hit)
    {
        string tag = hit.transform.tag;
        int effectIndex;
        switch (tag)
        {
            case "Enemy":
                effectIndex = 0;
                break;
            case "Ground":
                effectIndex = 1;
                break;
            case "Concrete":
                effectIndex = 2;
                break;
            case "AmmoBox":
                effectIndex = 5;
                break;
            case "Sand":
                effectIndex = 3;
                break;
            case "Weapon":
                effectIndex = 4;
                break;
            default:
                effectIndex = 0;
                break;
        }
        GameObject impacteffect = Instantiate(hitMarkers[effectIndex], hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impacteffect, 1f);
    }
}
