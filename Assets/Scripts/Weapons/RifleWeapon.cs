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
        if (character.RifleHandSocket)
        {
            transform.position = character.RifleHandSocket.position;
            transform.rotation = character.RifleHandSocket.rotation;
            character.characterState = CharacterState.Equipped_Rifle;
        }
    }

    public void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, canon.position, canon.rotation);
        // bullet.transform.SetPositionAndRotation(canon.position, canon.rotation);
        
        bullet.GetComponent<Rigidbody>().AddForce(canon.forward * shotVelocity, ForceMode.Impulse);
    }
}
