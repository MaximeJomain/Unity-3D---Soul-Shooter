using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RifleWeapon : Weapon
{
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform canon;

    [SerializeField]
    private float shotVelocity;

    public override void Equip(Character character)
    {
        transform.position = character.HandSocket.position;

        character.characterState = CharacterState.Equipped_Rifle;
    }

    public void Shoot()
    {
        var bullet = Instantiate(bulletPrefab);
        bullet.transform.SetPositionAndRotation(canon.position, bulletPrefab.transform.rotation);
        
        bullet.GetComponent<Rigidbody>().AddForce(canon.forward * shotVelocity, ForceMode.Impulse);
    }
}
