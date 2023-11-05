using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleWeapon : Weapon
{
    public override void Equip(Character character)
    {
        transform.position = character.HandSocket.position;

        character.CharacterState = CharacterState.Equipped_Rifle;
    }
}
