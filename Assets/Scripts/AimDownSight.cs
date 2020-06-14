using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDownSight : MonoBehaviour
{
    [SerializeField]
    private float adsFOV = 50f;

    private Camera _fpsCam;
    private Animator _animator;

    private bool status = false;
    private float prevFOV;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("No Animator!");
        }
    }

    public void useFunction()
    {
        status = !status;
        _animator.SetBool("isAds", status);
        if (status)
        {
            StartCoroutine(OnADS());
        }
        else
        {
            OnUnAds();
        }
    }

    // for interrupting events: drop, switch
    public void resetFunction()
    {
        status = false;
        OnUnAds();
    }
    IEnumerator OnADS()
    {
        prevFOV = _fpsCam.fieldOfView;
        yield return new WaitForSeconds(0.2f);
        if (status) // fix execution order problem caused by coroutines.
        {
            UIManager.instance.setCrosshair(false);
            _fpsCam.fieldOfView = adsFOV;
        } 
    }
    void OnUnAds()
    {
        UIManager.instance.setCrosshair(true);
        _fpsCam.fieldOfView = prevFOV;
    }


    public bool isUsing()
    {
        return status;
    }

    private void OnEnable()
    {
        status = false;
        _animator.SetBool("isAds", status);
    }

    private void OnDisable()
    {
        _animator.SetBool("isAds", false);
    }

    public void assignCameras(Camera FPSCam)
    {
        _fpsCam = FPSCam;
    }
}
