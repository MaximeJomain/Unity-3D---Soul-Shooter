using System;
using Unity.VisualScripting;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    [SerializeField]
    protected float _damage;

    public virtual void Equip(Character character)
    {
        transform.position = character.HandSocket.position;
        transform.rotation = character.HandSocket.rotation;
    }
}
