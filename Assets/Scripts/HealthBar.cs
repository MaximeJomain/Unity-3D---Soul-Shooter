using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
// ReSharper disable CompareOfFloatsByEqualityOperator

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider _healthBar;
    
    [SerializeField]
    private Slider _easeHealthBar;
    
    private Transform _playerCamera;
    
    [SerializeField]
    private float _targetHealth;


    private void Start()
    {
        // TODO get player health
        _targetHealth = 100f;
        _playerCamera = Camera.main!.transform;
    }

    private void Update()
    {
        if (_healthBar.value != _targetHealth)
        {
            _healthBar.value = _targetHealth;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10f);
        }

        if (_easeHealthBar.value != _healthBar.value)
        {
            _easeHealthBar.value = Mathf.Lerp(_easeHealthBar.value, _targetHealth, 0.025f);
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(_playerCamera);
    }

    private void TakeDamage(float damage)
    {
        _targetHealth -= damage;
    }
}
