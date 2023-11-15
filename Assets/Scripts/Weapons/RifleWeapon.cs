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

    private Transform _playerTransform;

    public override void Equip(Character character)
    {
        if (character.RifleHandSocket)
        {
            transform.position = character.RifleHandSocket.position;
            transform.rotation = character.RifleHandSocket.rotation;
            character.characterState = CharacterState.Equipped_Rifle;
            _playerTransform = character.transform;
        }
    }

    public void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, canon.position, _playerTransform.rotation);
        bullet.SetDamage(Damage);
        
        bullet.GetComponent<Rigidbody>().AddForce(_playerTransform.forward * shotVelocity, ForceMode.Impulse);
    }
}
