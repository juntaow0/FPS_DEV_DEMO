using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    [SerializeField]
    private GameObject _crosshair;
    [SerializeField]
    private GameObject _scope;
    [SerializeField]
    private TextMeshProUGUI _score;
    [SerializeField]
    private TextMeshProUGUI _reserveAmmo;
    [SerializeField]
    private TextMeshProUGUI _currentAmmo;


    private void Awake()
    {
        if (instance!=null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public void setCrosshair(bool active) {
        _crosshair.SetActive(active);
    }
    public void setScope(bool active)
    {
        _scope.SetActive(active);
    }

    public void updateCurrentAmmo(int amount)
    {
        _currentAmmo.SetText(amount + " /");
    }

    public void updateReserveAmmo(int amount)
    {
        _reserveAmmo.SetText(amount.ToString());
    }

    public void updateScore(int amount)
    {
        _score.SetText(amount.ToString());
    }
}
