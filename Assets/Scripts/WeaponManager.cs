using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class WeaponManager : MonoBehaviour
{
    private int selectedWeapon = 0;
    private int _weaponCount = 0;
    private Player _player;
    private Animator _animator;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_weaponCount > 0)
        {
            int preWeapon = selectedWeapon;
            if (Mouse.current.scroll.y.ReadValue() > 0f)
            {
                if (selectedWeapon >= transform.childCount - 1)
                {
                    selectedWeapon = 0;
                }
                else
                {
                    selectedWeapon++;
                }
            }
            if (Mouse.current.scroll.y.ReadValue() < 0f)
            {
                if (selectedWeapon <= 0)
                {
                    selectedWeapon = transform.childCount - 1;
                }
                else
                {
                    selectedWeapon--;
                }
            }

            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                selectedWeapon = 0;
            }
            if (Keyboard.current.digit2Key.wasPressedThisFrame && _weaponCount >= 2)
            {
                selectedWeapon = 1;
            }
            if (Keyboard.current.digit3Key.wasPressedThisFrame && _weaponCount >= 3)
            {
                selectedWeapon = 2;
            }
            if (Keyboard.current.digit4Key.wasPressedThisFrame && _weaponCount >= 4)
            {
                selectedWeapon = 3;
            }

            if (preWeapon != selectedWeapon)
            {
                SelectWeapon();
            }
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                _player.updateWeapon(weapon);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }

    void adjustTransform(Transform weapon)
    {
        weapon.localPosition = Vector3.zero;
        weapon.localRotation = Quaternion.Euler(0, 90, 0);
    }

    void resetTransform(Transform weapon)
    {
        weapon.localPosition = new Vector3(-0.483f, 0.129f, 0.115f);
        weapon.localRotation = Quaternion.identity;
    }

    public void pickUpWeapon(Transform weapon)
    {
        weapon.parent = transform;
        weapon.gameObject.SetActive(false);
        weapon.gameObject.layer = LayerMask.NameToLayer("WeaponView");
        weapon.GetChild(0).gameObject.layer = LayerMask.NameToLayer("WeaponView");
        weapon.GetComponent<Rigidbody>().isKinematic = true;
        weapon.GetComponent<BoxCollider>().isTrigger = true;
        adjustTransform(weapon);
        weapon.GetComponent<Weapon>().assignAnimator(_animator);
        _weaponCount++;
        SelectWeapon();
    }

    public void dropWeapon(Transform weapon)
    {
        weapon.gameObject.SetActive(false);
        weapon.gameObject.layer = 0;
        weapon.GetChild(0).gameObject.layer = 0;
        resetTransform(weapon);
        weapon.transform.parent = null;
        weapon.gameObject.SetActive(true);
        Rigidbody rbWeapon = weapon.GetComponent<Rigidbody>();
        weapon.GetComponent<BoxCollider>().isTrigger = false;
        rbWeapon.isKinematic = false;
        rbWeapon.AddForce(transform.forward * 6, ForceMode.VelocityChange);
        _weaponCount--;
        
        // if still has weapon, switch to it
        if (_weaponCount > 0)
        {
            if (_weaponCount == selectedWeapon)
            {
                selectedWeapon = _weaponCount - 1;
            }
            SelectWeapon();
        }
        else
        {
            _player.OnDropAllWweapon();
        }
    }
}
