using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    public float Health;
    
    private bool _isAlive = true;

    private Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_isAlive)
        {
            if (Health <= 0f)
            {
                Die();
            }      
        }
    }

    private void Die()
    {
        _isAlive = false;
        _animator.SetTrigger("death");
        Destroy(gameObject, 5f);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }
}
