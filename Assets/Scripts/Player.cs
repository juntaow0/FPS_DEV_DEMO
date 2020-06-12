using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private float nextTimeToFire = 0f;
    private PlayerInputClass _pi;
    private WeaponManager _wm;
    private Weapon _currentWeapon;
    private bool hasWeapon = false;
    private Vector2 _center;

    private void Awake()
    {
        _pi = new PlayerInputClass();
        _wm = FindObjectOfType<WeaponManager>();
        _center = new Vector3(0.5f, 0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            RaycastHit hit;
            Ray origin = Camera.main.ViewportPointToRay(_center);
            if (Physics.Raycast(origin, out hit, 5))
            {
                if (hit.transform.tag == "Weapon")
                {
                    // add disabled weapon to weapon manager
                    _wm.pickUpWeapon(hit.transform);
                }else if (hit.transform.tag == "AmmoBox")
                {
                    if (_currentWeapon != null)
                    {
                        _currentWeapon.getAmmo();
                    }
                }
            }
        }

        // use weapon
        if (hasWeapon)
        {
            if (Keyboard.current.gKey.wasPressedThisFrame)
            {
                _currentWeapon.stopSecondaryFunction();
                _wm.dropWeapon(_currentWeapon.transform);
                return;
            }
            if (_currentWeapon._isReloading || _currentWeapon._isSwapping)
            {
                return;
            }
            if (_currentWeapon._currentAmmo <= 0 && _currentWeapon._reserveAmmo > 0)
            {
                _currentWeapon.stopSecondaryFunction();
                _currentWeapon.reload(true);
                return;
            }
            if (_pi.Player.Reload.triggered && _currentWeapon._currentAmmo < _currentWeapon._magSize && _currentWeapon._reserveAmmo > 0)
            {
                _currentWeapon.stopSecondaryFunction();
                _currentWeapon.reload(false);
                return;
            }
            if (_currentWeapon.isAutomatic)
            {
                if (Mouse.current.leftButton.isPressed && Time.time >= nextTimeToFire && _currentWeapon._currentAmmo > 0)
                {
                    nextTimeToFire = Time.time + 1f / _currentWeapon._fireRate;
                    _currentWeapon.Shoot();
                }
            }
            else
            {
                if (_pi.Player.Fire.triggered && Time.time >= nextTimeToFire && _currentWeapon._currentAmmo > 0)
                {
                    nextTimeToFire = Time.time + 1f / _currentWeapon._fireRate;
                    _currentWeapon.Shoot();
                }
            }
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                _currentWeapon.secondayFunction();
            }
        }      
    }

    public void updateWeapon(Transform weapon)
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.stopSecondaryFunction();
        }
        _currentWeapon = weapon.GetComponent<Weapon>();
        _currentWeapon.OnSwap();
        hasWeapon = true;
        nextTimeToFire = 0;
    }

    public void OnDropAllWweapon()
    {
        hasWeapon = false;
        _currentWeapon = null;
    }

    private void OnEnable()
    {
        _pi.Player.Fire.Enable();
        _pi.Player.Reload.Enable();
    }

    private void OnDisable()
    {
        _pi.Player.Fire.Disable();
        _pi.Player.Reload.Disable();
    }
}