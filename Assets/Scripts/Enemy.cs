using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour,IDamage
{
    [SerializeField]
    private int _maxHealth = 100;
    private int _currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Destroy(this.gameObject);
    }
}
