using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHolder : MonoBehaviour
{
    private int _selectedWeaponIndex = 0;
    private int _weaponCount = 0;
    private Player _player;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }
    void Update()
    {
        if (_weaponCount > 0)
        {
            int preWeapon = _selectedWeaponIndex;
            mouseWheelSelect();
            keyboardSelect();

            if (preWeapon != _selectedWeaponIndex)
            {
                SelectWeapon();
            }
        }
    }
    void mouseWheelSelect()
    {
        if (Mouse.current.scroll.y.ReadValue() > 0f)
        {
            if (_selectedWeaponIndex >= transform.childCount - 1)
            {
                _selectedWeaponIndex = 0;
            }
            else
            {
                _selectedWeaponIndex++;
            }
        }
        if (Mouse.current.scroll.y.ReadValue() < 0f)
        {
            if (_selectedWeaponIndex <= 0)
            {
                _selectedWeaponIndex = transform.childCount - 1;
            }
            else
            {
                _selectedWeaponIndex--;
            }
        }
    }
    void keyboardSelect()
    {
        
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            _selectedWeaponIndex = 0;
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame && _weaponCount >= 2)
        {
            _selectedWeaponIndex = 1;
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame && _weaponCount >= 3)
        {
            _selectedWeaponIndex = 2;
        }
        if (Keyboard.current.digit4Key.wasPressedThisFrame && _weaponCount >= 4)
        {
            _selectedWeaponIndex = 3;
        }
    }
    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == _selectedWeaponIndex)
            {
                adjustTransform(weapon); // fix transform problem caused by disabling animator
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
        weapon.parent = transform;
        weapon.localPosition = Vector3.zero;
        weapon.localRotation = Quaternion.Euler(0, 90, 0);
    }

    void resetTransform(Transform weapon)
    {
        weapon.localPosition = new Vector3(-0.483f, 0.129f, 0.115f);
        weapon.localRotation = Quaternion.identity;
        weapon.transform.parent = null;
    }

    void adjustLayer(GameObject weapon)
    {
        weapon.layer = LayerMask.NameToLayer("WeaponView");
        GameObject muzzleFlash = weapon.transform.GetChild(0).gameObject;
        muzzleFlash.layer = LayerMask.NameToLayer("WeaponView");
        foreach (Transform effect in muzzleFlash.transform)
        {
            effect.gameObject.layer = LayerMask.NameToLayer("WeaponView");
        }
    }

    void resetLayer(GameObject weapon)
    {
        weapon.layer = 0;
        GameObject muzzleFlash = weapon.transform.GetChild(0).gameObject;
        muzzleFlash.layer = 0;
        foreach (Transform effect in muzzleFlash.transform)
        {
            effect.gameObject.layer = 0;
        }
    }

    public void pickUpWeapon(Transform weapon)
    {
        weapon.gameObject.SetActive(false);
        adjustTransform(weapon);
        adjustLayer(weapon.gameObject);

        weapon.GetComponent<Rigidbody>().isKinematic = true;
        BoxCollider[] colliders = weapon.GetComponents<BoxCollider>();
        foreach (BoxCollider c in colliders)
        {
            c.enabled = false;
        }
        if (_weaponCount < 1)
        {
            SelectWeapon();
        }
        _weaponCount++;
    }

    public void dropWeapon(Transform weapon)
    {
        weapon.gameObject.SetActive(false);
        resetTransform(weapon);
        resetLayer(weapon.gameObject);
        weapon.GetComponent<Animator>().enabled = false;
        weapon.gameObject.SetActive(true);
        Rigidbody rbWeapon = weapon.GetComponent<Rigidbody>();
        BoxCollider[] colliders = weapon.GetComponents<BoxCollider>();
        foreach (BoxCollider c in colliders)
        {
            c.enabled = true;
        }
        rbWeapon.isKinematic = false;
        rbWeapon.AddForce(transform.forward * 300f * Time.deltaTime, ForceMode.VelocityChange);
        _weaponCount--;
        // if still has weapon, switch to it
        if (_weaponCount > 0)
        {
            if (_weaponCount == _selectedWeaponIndex)
            {
                _selectedWeaponIndex = _weaponCount - 1;
            }
            SelectWeapon();
        }
        else
        {
            _player.OnDropAllWweapon();
        }
    }
}
