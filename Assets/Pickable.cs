using System;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    private Weapon _weaponParent;
    private SphereCollider _hoverCollider;
    private float _elapsedTime;
    private Vector3 _startPosition;
    private const float _timeConstant = 1.5f;
    private const float _sinAmplitude = 0.2f;

    private void Awake()
    {
        _weaponParent = transform.parent.GetComponent<Weapon>();
        _hoverCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        _elapsedTime = 0f;
        _startPosition = _weaponParent.transform.position;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        _weaponParent.transform.position = _startPosition + new Vector3(0f , TransformedSin(), 0f);
    }

    private float TransformedSin()
    {
        return _sinAmplitude * Mathf.Sin(_elapsedTime * _timeConstant);
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
