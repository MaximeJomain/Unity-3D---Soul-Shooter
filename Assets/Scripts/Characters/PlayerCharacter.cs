using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerCharacter : Character
{
    private CharacterActions _characterActions;
    
    // MOVEMENT
    [SerializeField]
    private float cameraSensitivity = 3f;
    [SerializeField]
    private float rotationSpeed = 5f;
    private Transform _followTransform;
    private Vector2 _move;
    private Vector2 _look;
    private float _movementSpeed;

    // SWORD COMBO
    private float _attackCombo = 1;
    private float _maxCombo = 3;
    private float _comboTimeFrame = 0.7f;
    private float _comboElapsedTime;

    protected override void Awake()
    {
        base.Awake();
        
        _followTransform = GameObject.Find("Camera Follow Target").transform;
    }

    protected override void Start()
    {
        base.Start();
        
        _animator.SetInteger("characterState", (int)characterState);
        _movementSpeed = MovementSpeed;
    }
    
    private void OnEnable()
    {
        if (_characterActions == null)
        {
            _characterActions = new CharacterActions();

            _characterActions.Player.Move.performed += input => _move = input.ReadValue<Vector2>();
            _characterActions.Player.Look.performed += input => _look = input.ReadValue<Vector2>();
            _characterActions.Player.Fire.performed += _ => Fire();
        }
        
        _characterActions.Enable();
    }

    protected override void Update()
    {
        if (!IsAlive) return;

        if (_actionState != ActionState.IsAttacking)
        {
            _comboElapsedTime += Time.deltaTime;
        }

        if (Health <= 0f)
        {
            Die();
        }

        HandleCamera();

        HandleMovement();
    }
    
    private void HandleCamera()
    {
        _followTransform.position = new Vector3(transform.position.x, _followTransform.position.y, transform.position.z);

        // Rotate the follow target based on input
        _followTransform.rotation *= Quaternion.AngleAxis(_look.x * cameraSensitivity, Vector3.up);
        _followTransform.rotation *= Quaternion.AngleAxis(_look.y * cameraSensitivity, Vector3.right);

        var localAngles = _followTransform.localEulerAngles;
        localAngles.z = 0f;

        var localAngleX = _followTransform.localEulerAngles.x;

        if (localAngleX > 180 && localAngleX < 340)
        {
            localAngles.x = 340;
        }
        else if (localAngleX < 180 && localAngleX > 40)
        {
            localAngles.x = 40f;
        }

        _followTransform.transform.localEulerAngles = localAngles;
    }

    private void HandleMovement()
    {
        if (_actionState == ActionState.IsAttacking)
            _movementSpeed = MovementSpeed * 0.3f;
        else
            _movementSpeed = MovementSpeed;

        if (_move != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, _followTransform.rotation.eulerAngles.y, 0f);

            Vector3 moveDirection = targetRotation * new Vector3(_move.x, 0f, _move.y);
            _rigidbody.velocity = moveDirection * _movementSpeed;

            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        Vector2 movement = new Vector2(Mathf.Abs(_move.x), Math.Abs(_move.y));
        _animator.SetFloat("movement", movement.magnitude);
    }

    private void OnMove(InputValue value)
    {
        _move = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        _look = value.Get<Vector2>();
    }

    private void Fire()
    {
        if (_actionState != ActionState.Unoccupied)
            return;

        // ONE-HANDED SWORD
        if (characterState == CharacterState.Equipped_OneHanded)
        {
            _actionState = ActionState.IsAttacking;

            if (_comboElapsedTime > _comboTimeFrame)
            {
                _attackCombo = 1;
            }

            _comboElapsedTime = 0;

            _animator.SetTrigger("Attack" + _attackCombo);

            _attackCombo++;

            if (_attackCombo > _maxCombo)
            {
                _attackCombo = 1;
            }
        }

        // RIFLE
        else if (characterState == CharacterState.Equipped_Rifle)
        {
            RifleWeapon rifle = _weapon.GetComponent<RifleWeapon>();
            if (rifle)
            {
                rifle.Shoot();
                _animator.SetTrigger("Shoot");
            }
        }
    }

    private void OnDodge()
    {
        if (_actionState == ActionState.Unoccupied)
        {
            _actionState = ActionState.IsDodging;
            _animator.SetTrigger("Roll");
        }
    }

    private void DodgeStart()
    {
        _isInvincible = true;
    }

    private void DodgeEnd()
    {
        _isInvincible = false;
    }
}
