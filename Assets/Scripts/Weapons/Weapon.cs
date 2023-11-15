using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


public class Weapon : MonoBehaviour
{
    [FormerlySerializedAs("damage")]
    [FormerlySerializedAs("_damage")]
    [SerializeField]
    public float Damage;

    public virtual void Equip(Character character)
    {
        transform.position = character.HandSocket.position;
        transform.rotation = character.HandSocket.rotation;
    }
}
