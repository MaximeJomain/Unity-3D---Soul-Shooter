using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon : Weapon
{
    private Collider _attackCollider;

    private void Awake()
    {
        _attackCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        _attackCollider.enabled = false;
    }

    public override void Equip(Character character)
    {
        base.Equip(character);

        character.characterState = CharacterState.Equipped_OneHanded;
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
