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

    private bool _readyToShoot;
    private bool allowInvoke;
    private float timeBetweenShooting;

    private void Start()
    {
        _readyToShoot = true;
        allowInvoke = true;
        timeBetweenShooting = 0.1f;
    }

    public override void Equip(Character character)
    {
        if (character.RifleHandSocket)
        {
            transform.parent = character.RifleHandSocket;
            transform.position = character.RifleHandSocket.position;
            transform.rotation = character.RifleHandSocket.rotation;
            character.characterState = CharacterState.Equipped_Rifle;
        }
    }

    public void Shoot(Vector3 mouseWorldPosition)
    {
        if (_readyToShoot)
        {
            _readyToShoot = false;
            
            Vector3 aimDirection = (mouseWorldPosition - canon.position).normalized;
            var bullet = Instantiate(bulletPrefab, canon.position, Quaternion.LookRotation(aimDirection, Vector3.up));
            bullet.SetDamage(Damage);

            if (allowInvoke)
            {
                Invoke("ResetShot", timeBetweenShooting);
                allowInvoke = false;
            }
        }
    }

    private void ResetShot()
    {
        _readyToShoot = true;
        allowInvoke = true;
    }
}
