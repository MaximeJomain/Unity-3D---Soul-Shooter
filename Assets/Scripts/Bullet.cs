using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _damage;

    private bool _hasHit;

    private void Start()
    {
        _hasHit = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!_hasHit)
        {
            Character enemy = other.gameObject.GetComponent<Character>();
            if (enemy)
            {
                enemy.TakeDamage(_damage);
                _hasHit = true;
            }
        }
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }
}
