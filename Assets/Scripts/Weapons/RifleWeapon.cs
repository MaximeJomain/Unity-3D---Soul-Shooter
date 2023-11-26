using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RifleWeapon : Weapon
{
    [SerializeField]
    private Bullet bulletPrefab;

    [SerializeField]
    private Transform canon;

    [SerializeField]
    private float shotVelocity;
    
    public override void Equip(Character character)
    {
        if (character.RifleHandSocket)
        {
            transform.position = character.RifleHandSocket.position;
            transform.rotation = character.RifleHandSocket.rotation;
            character.characterState = CharacterState.Equipped_Rifle;
        }
    }

    public void Shoot(Vector3 mouseWorldPosition)
    {
        Vector3 aimDirection = (mouseWorldPosition - canon.position).normalized;
        var bullet = Instantiate(bulletPrefab, canon.position, Quaternion.LookRotation(aimDirection, Vector3.up));
        bullet.SetDamage(Damage);
    }
}
