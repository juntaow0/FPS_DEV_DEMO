﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDownSight : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void assignAnimator(Animator _animator)
    {
        animator = _animator;
    }

    public bool isUsing()
    {
        return false;
    }
}
