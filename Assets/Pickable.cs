using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    private Weapon _weaponParent;
    private SphereCollider _hoverCollider;

    private void Awake()
    {
        _weaponParent = transform.parent.GetComponent<Weapon>();
        _hoverCollider = GetComponent<SphereCollider>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player)
        {
            player.SetOverlapWeapon(_weaponParent);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player)
        {
            player.SetOverlapWeapon(null);
        }
    }

    public void SetColliderEnabled(bool value)
    {
        _hoverCollider.enabled = value;
    }
    
}
