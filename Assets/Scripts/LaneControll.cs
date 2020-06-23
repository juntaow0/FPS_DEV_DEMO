using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class LaneControll : MonoBehaviour
{
    [SerializeField]
    private Transform[] _targets;

    private int currentTarget = 4;

    private Vector3 disableState = new Vector3(80, 0, 0);

    private bool isRunning = false;

    private void Awake()
    {
        disableTargets(_targets);
    }

    public void useButton()
    {
        if (!isRunning)
        {
            AudioManager.instance.Play("buttons");
            if (currentTarget < 4)
            {
                disableTarget(_targets[currentTarget]);
            }
            currentTarget--;
            if (currentTarget < 0)
            {
                currentTarget = 4;
            }
            if (currentTarget != 4)
            {
                enableTarget(_targets[currentTarget]);
            }
        }
    }

    void enableTarget(Transform target)
    {
        StartCoroutine( enableRoutine(target));
    }

    IEnumerator enableRoutine(Transform target)
    {
        isRunning = true;
        float slerpValue = 0;
        float turnSpeed = 1f;
        while (slerpValue <= 1f)
        {
            slerpValue += Time.deltaTime * turnSpeed;
            target.rotation = Quaternion.Slerp(target.rotation, Quaternion.identity, slerpValue);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        isRunning = false;
    }

    void disableTarget(Transform target)
    {
        StartCoroutine(disableRoutine(target));
    }

    IEnumerator disableRoutine(Transform target)
    {
        isRunning = true;
        float slerpValue = 0;
        float turnSpeed = 1f;
        while (slerpValue <= 1f)
        {
            slerpValue += Time.deltaTime * turnSpeed;
            target.rotation = Quaternion.Slerp(target.rotation, Quaternion.Euler(disableState), slerpValue);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        isRunning = false;
    }

    void disableTargets(Transform[] targets)
    {
        foreach (Transform target in targets)
        {
            target.rotation = Quaternion.Euler(disableState);
        }  
    }
}


