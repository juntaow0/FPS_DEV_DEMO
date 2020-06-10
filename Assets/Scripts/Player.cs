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
        _currentWeapon = _wm.GetComponentInChildren<Weapon>(false);
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
                    pickUpWeapon(hit);
                }else if (hit.transform.tag == "AmmoBox")
                {
                    _currentWeapon.getAmmo();
                }
            }
        }
        if (hasWeapon)
        {
            if (Keyboard.current.gKey.wasPressedThisFrame)
            {
                dropWeapon();
                return;
            }
            if (_currentWeapon._isReloading)
            {
                return;
            }
            if (_currentWeapon._currentAmmo <= 0 && _currentWeapon._reserveAmmo > 0)
            {
                _currentWeapon.reload(true);
                return;
            }
            if (_pi.Player.Reload.triggered && _currentWeapon._currentAmmo < _currentWeapon._magSize && _currentWeapon._reserveAmmo > 0)
            {
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
        }      
    }

    void dropWeapon()
    {
        Debug.Log("drop a weapon");
        _currentWeapon.gameObject.SetActive(false);
        _currentWeapon.gameObject.layer = 0;
        _currentWeapon.transform.GetChild(0).gameObject.layer = 0;
        _currentWeapon.transform.localPosition = new Vector3(-0.483f, 0.129f, 0.115f);
        _currentWeapon.transform.localRotation = Quaternion.identity;
        _currentWeapon.gameObject.SetActive(true);
        Rigidbody wrb = _currentWeapon.GetComponent<Rigidbody>();
        BoxCollider wbc = _currentWeapon.GetComponent<BoxCollider>();
        _currentWeapon.transform.parent = null;
        wrb.isKinematic = false;
        wbc.isTrigger = false;
        wrb.AddForce(transform.forward*6,ForceMode.VelocityChange);
        _wm.removeWeapon();
        if (_wm.transform.childCount < 1)
        {
            hasWeapon = false;
        }
        else
        {
            _currentWeapon = _wm.GetComponentInChildren<Weapon>(false);
        }
    }

    void pickUpWeapon(RaycastHit hit)
    {
        
        hit.transform.parent = _wm.transform;
        _currentWeapon = hit.transform.GetComponent<Weapon>();
        _currentWeapon.gameObject.layer = LayerMask.NameToLayer("WeaponView");
        _currentWeapon.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("WeaponView");
        Rigidbody wrb = _currentWeapon.GetComponent<Rigidbody>();
        BoxCollider wbc = _currentWeapon.GetComponent<BoxCollider>();
        wrb.isKinematic = true;
        wbc.isTrigger = true;
        adjustTransform();
        hasWeapon = true;
        _wm.addWeapon();
    }


    void adjustTransform()
    {
        _currentWeapon.transform.localPosition = Vector3.zero;
        _currentWeapon.transform.localRotation = Quaternion.Euler(0, 90, 0);
    }

    public void switchWeapon()
    {
        _currentWeapon = _wm.GetComponentInChildren<Weapon>(false);
        nextTimeToFire = 0;
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
