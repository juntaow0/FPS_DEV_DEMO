using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private int selectedWeapon = 0;

    private void Awake()
    {
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

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            selectedWeapon = 0;
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame && transform.childCount >= 2)
        {
            selectedWeapon = 1;
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame && transform.childCount >= 3)
        {
            selectedWeapon = 2;
        }
        if (Keyboard.current.digit4Key.wasPressedThisFrame && transform.childCount >= 4)
        {
            selectedWeapon = 3;
        }

        if (preWeapon != selectedWeapon)
        {
            SelectWeapon();
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
}
