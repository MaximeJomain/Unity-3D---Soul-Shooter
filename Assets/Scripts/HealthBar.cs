using UnityEngine;
using UnityEngine.UI;
// ReSharper disable CompareOfFloatsByEqualityOperator

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider _healthBar;
    
    [SerializeField]
    private Slider _easeHealthBar;
    
    [SerializeField]
    private Enemy _enemy;
    
    private float _targetHealth;

    private Transform _playerCamera;


    private void Awake()
    {
        _playerCamera = Camera.main!.transform;
    }

    private void Update()
    {
        if (_healthBar.value != _enemy.Health)
        {
            _healthBar.value = _enemy.Health;
        }

        if (_easeHealthBar.value != _healthBar.value)
        {
            _easeHealthBar.value = Mathf.Lerp(_easeHealthBar.value, _enemy.Health, 0.025f);
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + _playerCamera.forward);
    }
}
