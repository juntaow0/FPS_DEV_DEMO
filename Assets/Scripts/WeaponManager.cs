using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private int selectedWeapon = 0;
    private Player _player;
    private int _weaponCount = 0;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
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

        if (_weaponCount > 0)
        {
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
        }

        if (preWeapon != selectedWeapon)
        {
            SelectWeapon();
            _player.switchWeapon();
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
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }

    public void addWeapon()
    {
        _weaponCount++;
        SelectWeapon();
    }

    public void removeWeapon()
    {
        _weaponCount--;
        if (_weaponCount > 0)
        {
            SelectWeapon();
        }
    }
}
