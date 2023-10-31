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
    
    [SerializeField]
    private Character _target;

    [SerializeField]
    private bool _isFacingPlayer;
    
    private Transform _playerCamera;


    private void Awake()
    {
        _playerCamera = Camera.main!.transform;
    }

    private void Start()
    {
        _healthBar.maxValue = _target.Health;
        _easeHealthBar.maxValue = _target.Health;
    }

    private void Update()
    {
        if (_healthBar.value != _target.Health)
        {
            _healthBar.value = _target.Health;
        }

        if (_easeHealthBar.value != _healthBar.value)
        {
            _easeHealthBar.value = Mathf.Lerp(_easeHealthBar.value, _target.Health, 0.025f);
        }
    }

    private void LateUpdate()
    {
        if (_isFacingPlayer)
            transform.LookAt(transform.position + _playerCamera.forward);
    }
}
