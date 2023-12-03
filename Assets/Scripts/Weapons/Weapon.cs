using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    public float Damage;

    [SerializeField]
    private GameObject _pickable;
    
    public virtual void Equip(Character character)
    {
        _pickable.SetActive(false);
    }
}
