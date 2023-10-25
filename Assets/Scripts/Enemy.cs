using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float health;
    
    private bool _isAlive = true;

    private void Update()
    {
        if (_isAlive)
        {
            if (health <= 0f)
            {
                Die();
            }      
        }
    }

    private void Die()
    {
        _isAlive = false;
        Debug.Log(gameObject.name + "died");
        Destroy(gameObject, 3f);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
