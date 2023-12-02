using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon : Weapon
{
    private CapsuleCollider _attackCollider;

    private void Awake()
    {
        _attackCollider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        _attackCollider.enabled = false;
    }

    public override void Equip(Character character)
    {
        base.Equip(character);
        
        if (character.SwordSocket)
        {
            transform.parent = character.SwordSocket;
            transform.position = character.SwordSocket.position;
            transform.rotation = character.SwordSocket.rotation;
            character.CharacterState = CharacterState.Equipped_OneHandedSword;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Character target = other.GetComponent<Character>();
        if (target)
        {
            target.TakeDamage(Damage);
        }
    }
}
