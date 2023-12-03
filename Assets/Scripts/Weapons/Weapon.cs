using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    public float Damage;

    [SerializeField]
    private GameObject _pickable;

    private bool isEquipped;
    public bool IsEquipped => isEquipped;

    public virtual void Equip(Character character)
    {
        isEquipped = true;
        _pickable.SetActive(false);
    }
}
