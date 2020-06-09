using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private int _damage = 100;
    [SerializeField]
    private float _reloadTime = 2f;
    [SerializeField]
    private float _fireRate = 5f;
    [SerializeField]
    private int _magSize = 5;
    [SerializeField]
    private int _reserveAmmo = 30;
    [SerializeField]
    private float _effectiveRange = 1000f;
    [SerializeField]
    private ParticleSystem _muzzleFlash;
    [SerializeField]
    private GameObject _impactEffect;

    private int _currentAmmo;
    private bool _isReloading = false;
    private float nextTimeToFire = 0f;
    private Vector3 _center;
    private PlayerInputClass _pi;

    private void Awake()
    {
        _pi = new PlayerInputClass();
        _currentAmmo = _magSize;
        _center = new Vector3(0.5f, 0.5f, 0);
    }

    private void OnEnable()
    {
        _isReloading = false;
        //animator.SetBool("reloading", false);
        _pi.Player.Fire.Enable();
        _pi.Player.Reload.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isReloading)
        {
            return;
        }
        if (_currentAmmo <= 0 && _reserveAmmo > 0)
        {
            StartCoroutine(ReloadRoutine(true));
            return;
        }
        if (_pi.Player.Reload.triggered && _currentAmmo < _magSize && _reserveAmmo>0)
        {
            StartCoroutine(ReloadRoutine(false));
            return;
        }
        if (_pi.Player.Fire.triggered && Time.time >= nextTimeToFire && _currentAmmo>0)
        {
            nextTimeToFire = Time.time + 1f / _fireRate;
            Shoot();
        }
    }

    void Shoot()
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

    IEnumerator ReloadRoutine(bool empty)
    {
        _isReloading = true;
        //animator.SetBool("reloading", true);
        yield return new WaitForSeconds(_reloadTime - 0.25f);
        //animator.SetBool("reloading", false);
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

    private void OnDisable()
    {
        _pi.Player.Fire.Disable();
        _pi.Player.Reload.Disable();
    }
}
