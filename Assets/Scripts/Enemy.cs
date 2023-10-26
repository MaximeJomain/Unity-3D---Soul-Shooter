using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    public float Health;
    
    [NonSerialized]
    public float Speed;
    
    public bool IsAlive { get; private set; }
    
    private Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        IsAlive = true;
        Speed = 0f;
    }

    private void Update()
    {
        if (IsAlive)
        {
            _animator.SetFloat("Speed", Speed);
            
            if (Health <= 0f)
            {
                Die();
            }      
        }
    }

    public void Attack()
    {
        _animator.SetTrigger("Attack");
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }
    
    private void Die()
    {
        IsAlive = false;
        _animator.SetTrigger("Death");
        Destroy(gameObject, 5f);
    }
}
