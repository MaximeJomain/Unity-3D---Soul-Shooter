using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private float sensitivity = 3f;

    [SerializeField]
    private float cameraSmooth = 0.3f;

    private Camera _camera;

    private Vector2 _lookVector;

    private Quaternion _targetRotation;

    private Quaternion _currentRotation;

    private Vector3 _direction;

    private float _rotX;

    private float _rotY;

    private void Awake()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }
    }

    private void Start()
    {
        float cameraToTargetDistance = Vector3.Distance(_camera.transform.position, transform.position);
        _direction = new Vector3(0f, -1.5f, cameraToTargetDistance);
        _camera.transform.position = transform.position - _direction;
    }

    void Update()
    {
        MoveCamera();
    }

    private void OnLook(InputValue value)
    {
        _lookVector = value.Get<Vector2>();
    }

    private void MoveCamera()
    {
        _rotX += _lookVector.y * sensitivity;
        _rotY += _lookVector.x * sensitivity;

        _rotX = Mathf.Clamp(_rotX, -85f, 85f);

        Vector3 vector = new Vector3(_rotX, _rotY, 0f);
        _targetRotation = Quaternion.Euler(vector);
        _currentRotation = Quaternion.Slerp(_currentRotation, _targetRotation, Time.smoothDeltaTime * cameraSmooth * 50f);
        _camera.transform.position = transform.position - _currentRotation * _direction;
        _camera.transform.LookAt(transform.position + new Vector3(0f, 1.5f, 0f));
    }
}
