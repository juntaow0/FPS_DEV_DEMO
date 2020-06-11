using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDownSight : MonoBehaviour
{
    [SerializeField]
    private float adsFOV = 50f;

    private GameObject crosshair;
    private Camera mainCam;
    private Animator animator;

    private bool status = false;
    private float prevFOV;
    // Start is called before the first frame update
    private void Awake()
    {
        crosshair = GameObject.Find("crosshair");
        mainCam = GameObject.Find("FPSCam").GetComponent<Camera>();
    }

    public void useFunction(string trigger)
    {
        status = !status;
        animator.SetBool("isAds", status);
        animator.SetTrigger(trigger);

        if (status)
        {
            StartCoroutine(OnADS());
        }
        else
        {
            OnUnAds();
        }
    }
    public void resetFunction()
    {
        status = false;
        OnUnAds();
    }

    IEnumerator OnADS()
    {
        yield return new WaitForSeconds(0.2f);
        crosshair.SetActive(false);
        prevFOV = mainCam.fieldOfView;
        mainCam.fieldOfView = adsFOV;
    }

    void OnUnAds()
    {
        crosshair.SetActive(true);
        mainCam.fieldOfView = prevFOV;
    }

    public void assignAnimator(Animator _animator)
    {
        animator = _animator;
    }

    public bool isUsing()
    {
        return status;
    }

    private void OnEnable()
    {
        status = false;
        if (transform.parent != null)
        {
            animator.SetBool("isAds", status);
        }
    }
}
