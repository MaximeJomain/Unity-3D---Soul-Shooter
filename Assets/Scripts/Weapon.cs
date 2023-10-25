using System;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private float _damage;  
    
    private Collider _attackCollider;

    private void Awake()
    {
        _attackCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        _attackCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit" + other.gameObject.name);
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.TakeDamage(_damage);
        }
        
    }

}
