using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamage
{
    [SerializeField]
    private int _maxHealth = 100;
    [SerializeField]
    private float _interactionDistance = 5f;
    [SerializeField]
    private WeaponHolder _weaponHolder;
    [SerializeField]
    private Camera _FPSCam;
    [SerializeField]
    private Camera _weaponCam;

    private int _score = 0;

    private int _currentHealth;
    private PlayerInputClass _pi;
    private WeaponController _currentWeapon;
    private bool hasWeapon = false;
    private void Awake()
    {
        _currentHealth = _maxHealth;
        _pi = new PlayerInputClass();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            RaycastHit hit;
            if (Physics.Raycast(_FPSCam.transform.position, _FPSCam.transform.forward, out hit, _interactionDistance))
            {
                if (hit.transform.tag == "Weapon")
                {
                    // add disabled weapon to weapon manager
                   _weaponHolder.pickUpWeapon(hit.transform);
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
                _weaponHolder.dropWeapon(_currentWeapon.transform);
                return;
            }
            if (_currentWeapon.isReloading() || _currentWeapon.isSwapping())
            {
                return;
            }
            if (_pi.Player.Reload.triggered)
            {
                _currentWeapon.reload();
                return;
            }
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                _currentWeapon.secondaryFunction();
            }
            if (_currentWeapon.isAutomatic())
            {
                if (Mouse.current.leftButton.isPressed)
                {
                    _currentWeapon.Fire();
                }
            }
            else
            {
                if (_pi.Player.Fire.triggered)
                {
                    if (_currentWeapon.isEmpty())
                    {
                        _currentWeapon.reload();
                        return;
                    }
                    _currentWeapon.Fire();
                }
            } 
        }      
    }
    public void updateWeapon(Transform weapon)
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.stopSecondaryFunction();
        }
        _currentWeapon = weapon.GetComponent<WeaponController>();
        _currentWeapon.assignCamera(_FPSCam,_weaponCam);
        _currentWeapon.assignOwner(this);
        _currentWeapon.OnSwap();
        hasWeapon = true;
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
    public void takeDamage(int amount)
    {
        _currentHealth -= amount;
        if (_currentHealth < 1)
        {
            die();
        }
    }
    private void die()
    {
        SceneManager.LoadScene("Playground");
    }

    public void addScore(int amount)
    {
        _score += amount;
        UIManager.instance.updateScore(_score);
    }
}