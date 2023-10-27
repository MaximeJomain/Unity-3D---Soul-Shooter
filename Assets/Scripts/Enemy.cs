using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    public float Health;

    [SerializeField]
    private Weapon _weapon;

    [SerializeField]
    private Transform _handSocket;
    
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

        GameObject weaponInstance = Instantiate(_weapon.gameObject, _handSocket);
        weaponInstance.transform.position = _handSocket.position;
        weaponInstance.transform.rotation = _handSocket.rotation;
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
