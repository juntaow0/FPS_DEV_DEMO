using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScopeIn : MonoBehaviour
{
    [SerializeField]
    private float scopedFOV = 15f;

    private Animator _animator;
    private Camera _weaponCam;
    private Camera _fpsCam;

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
        _animator.SetBool("isScoped", status);
        if (status)
        {
            StartCoroutine(OnScoped());
        }
        else
        {
            OnUnscoped();
        }
    }
    // for interrupting events: drop, switch
    public void resetFunction()
    {
        status = false;
        OnUnscoped();
    }

    public void autoRescope()
    {
        StartCoroutine(autoRescopeRoutine());
    }

    IEnumerator autoRescopeRoutine()
    {
        status = true;
        prevFOV = _fpsCam.fieldOfView;
        yield return new WaitForSeconds(0.96f);
        if (status)
        {
            _weaponCam.gameObject.SetActive(false);
            UIManager.instance.setScope(true);
            UIManager.instance.setCrosshair(false);
            _fpsCam.fieldOfView = scopedFOV;
        } 
    }

    IEnumerator OnScoped()
    {
        prevFOV = _fpsCam.fieldOfView;
        yield return new WaitForSeconds(0.22f);
        if (status) // fix execution order problem caused by coroutines.
        {
            _weaponCam.gameObject.SetActive(false);
            UIManager.instance.setScope(true);
            UIManager.instance.setCrosshair(false);
            _fpsCam.fieldOfView = scopedFOV;
        }
    }
    void OnUnscoped()
    {
        UIManager.instance.setScope(false);
        UIManager.instance.setCrosshair(true);
        _weaponCam.gameObject.SetActive(true);
        _fpsCam.fieldOfView = prevFOV;
    }
    private void OnEnable()
    {
        status = false;
        _animator.SetBool("isScoped", status);
    }
    private void OnDisable()
    {
        _animator.SetBool("isScoped", false);
    }
    public bool isUsing()
    {
        return status;
    }
    public void assignCameras(Camera FPSCam, Camera weaponcCam)
    {
        _fpsCam = FPSCam;
        _weaponCam = weaponcCam;
    }
}
