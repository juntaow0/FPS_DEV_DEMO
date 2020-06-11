using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScopeIn : MonoBehaviour
{
    [SerializeField]
    private float scopedFOV = 15f;

    private Animator animator;
    private GameObject scopeOverlay;
    private GameObject crosshair;
    private GameObject weaponCam;
    private Camera mainCam;

    private bool status = false;
    private float prevFOV;

    private void Awake()
    {
        scopeOverlay = GameObject.Find("scope");
        scopeOverlay.gameObject.SetActive(false);
        crosshair = GameObject.Find("crosshair");
        weaponCam = GameObject.Find("WeaponCam");
        mainCam = GameObject.Find("FPSCam").GetComponent<Camera>();
    }

    public void useFunction()
    {
        status = !status;
        animator.SetBool("isScoped", status);
        if (status)
        {
            StartCoroutine(OnScoped());
        }
        else
        {
            OnUnscoped();
        }
    }

    public void resetFunction()
    {
        status = false;
        OnUnscoped();
    }

    void OnUnscoped()
    {
        scopeOverlay.SetActive(false);
        weaponCam.SetActive(true);
        crosshair.SetActive(true);
        mainCam.fieldOfView = prevFOV;
    }

    IEnumerator OnScoped()
    {
        yield return new WaitForSeconds(0.27f);
        scopeOverlay.SetActive(true);
        weaponCam.SetActive(false);
        crosshair.SetActive(false);
        prevFOV = mainCam.fieldOfView;
        mainCam.fieldOfView = scopedFOV;
    }

    public void assignAnimator(Animator _animator)
    {
        animator = _animator;
    }

    private void OnEnable()
    {
        status = false;
        if (transform.parent != null)
        {
            animator.SetBool("isScoped", status);
        }
    }

    public bool isUsing()
    {
        return status;
    }
}
