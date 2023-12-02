using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    public float Damage;

    [SerializeField]
    private Pickable _pickable;
    
    public virtual void Equip(Character character)
    {
        _pickable.SetColliderEnabled(false);
    }
}
