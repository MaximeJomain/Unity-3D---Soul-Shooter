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
        Character target = other.GetComponent<Character>();
        if (target)
        {
            target.TakeDamage(_damage);
        }
        
    }

}
