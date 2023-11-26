using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _damage;
    private bool _hasHit;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float speed = 40f;
        _rigidbody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyCharacter enemyHit = other.GetComponent<EnemyCharacter>();
        if (enemyHit)
        {
            enemyHit.TakeDamage(_damage);
        }
        
        Destroy(gameObject);
    }

    public void SetDamage(float newDamage)
    {
        _damage = newDamage;
    }
}
