using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    [SerializeField]
    private GameObject _crosshair;
    [SerializeField]
    private GameObject _scope;

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
}
