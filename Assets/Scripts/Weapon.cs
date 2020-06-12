using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private string weaponName = "M4A1";
    [SerializeField]
    private int type = 0; //0=ads, 1=scope
    [SerializeField]
    private float _reloadTime = 2f;
    [SerializeField]
    private int _damage = 100;
    [SerializeField]
    private float _effectiveRange = 1000f;
    [SerializeField]
    private ParticleSystem _muzzleFlash;
    [SerializeField]
    private GameObject _impactEffect;

    private ScopeIn _si;
    private AimDownSight _ads;

    public bool isAutomatic = false;

    public float _fireRate = 5f;
    public int _magSize = 5;
    public int _reserveAmmo = 30;
    public int _currentAmmo;
    public bool _isReloading = false;
    public int _reserveAmmoCapacity = 30;

    private Vector3 _center;
    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        _currentAmmo = _magSize;
        _center = new Vector3(0.5f, 0.5f, 0);
    }

    public void reload(bool empty)
    {
        StartCoroutine(ReloadRoutine(empty));
    }

    public void Shoot()
    {
        _muzzleFlash.Play();
        _currentAmmo--;
        RaycastHit hit;
        Ray origin = Camera.main.ViewportPointToRay(_center);
        if (Physics.Raycast(origin, out hit, _effectiveRange))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.tag == "Enemy")
            {
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.takeDamage(_damage);
                }
            }
            else if (hit.transform.tag == "Destructible")
            {

            }
            GameObject impacteffect = Instantiate(_impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impacteffect, 1f);

            //if (hit.rigidbody != null)
            //{
            //    hit.rigidbody.AddForce(-hit.normal * impForce * Time.deltaTime);
            //}
        }
    }

    public void stopSecondaryFunction()
    {
        if (type == 0)
        {
            if (_ads.isUsing())
            {       
                _ads.resetFunction();
            } 
        }
        else
        {
            if (_si.isUsing())
            {
                _si.resetFunction();
            }
        }
    }

    IEnumerator ReloadRoutine(bool empty)
    {
        _isReloading = true;

        animator.SetBool("isReloading", true);

        yield return new WaitForSeconds(_reloadTime - 0.25f);

        animator.SetBool("isScoped", false);
        animator.SetBool("isAds", false);
        animator.SetBool("isReloading", false);

        yield return new WaitForSeconds(0.25f);
        if (empty)
        {
            if (_reserveAmmo >= _magSize)
            {
                _currentAmmo = _magSize;
                _reserveAmmo -= _magSize;
            }
            else
            {
                _currentAmmo = _reserveAmmo;
                _reserveAmmo = 0;
            }
        }
        else
        {
            int amount = _magSize - _currentAmmo;
            if (_reserveAmmo >= amount)
            {

                _currentAmmo = _magSize;
                _reserveAmmo -= amount;
            }
            else
            {
                _currentAmmo += _reserveAmmo;
                _reserveAmmo = 0;
            }  
        }
        _isReloading = false;
    }

    private void OnEnable()
    {
        _isReloading = false;
        if (animator != null)
        {
            animator.SetBool("isReloading", false);
        } 
    }

    private void OnDisable()
    {
        if (animator != null)
        {
            animator.SetBool("isReloading", false);
        }
    }



    public void getAmmo()
    {
        if (_reserveAmmo == 0)
        {
            _reserveAmmo = _reserveAmmoCapacity;
        }
        else
        {
            _reserveAmmo = _reserveAmmoCapacity - _magSize;
        }
    }

    public void assignAnimator(Animator _animator)
    {
        animator = _animator;
        if (type == 0)
        {
            _ads = GetComponent<AimDownSight>();
            _ads.assignAnimator(_animator);
        }
        else
        {
            _si = GetComponent<ScopeIn>();
            _si.assignAnimator(_animator);
        }
    }

    public void secondayFunction()
    {
        if (type == 0)
        {
            _ads.useFunction(weaponName);
        }
        else
        {
            _si.useFunction();
        }
    }
}
