using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerCharacter : Character
{
    #region Class Fields

    [SerializeField]
    private Transform _debugSphere;
    [SerializeField]
    private LayerMask _debugLayerMask;
    
    private CharacterActions _characterActions;
    private bool _isFiring;
    private bool _isAiming;
    private bool _hasAttacked;

    private Camera _mainCamera;
    private GameObject _moveCamera;
    private GameObject _aimCamera;

    private RifleWeapon _rifleWeapon;
    private Vector3 _mouseWorldPosition;
    
    // MOVEMENT
    [SerializeField]
    private float cameraSensitivity = 3f, aimSensitivity = 1.5f;
    private float _sensitivity;
    
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
    
    #endregion

    protected override void Awake()
    {
        base.Awake();
        
        _followTransform = GameObject.Find("Camera Follow Target").transform;
        _mainCamera = Camera.main;
        _moveCamera = GameObject.Find("Move Camera");
        _aimCamera = GameObject.Find("Aim Camera");
    }

    protected override void Start()
    {
        base.Start();
        
        _animator.SetInteger("characterState", (int)characterState);
        _movementSpeed = MovementSpeed;
        _aimCamera.SetActive(false);
        _sensitivity = cameraSensitivity;
    }
    
    private void OnEnable()
    {
        if (_characterActions == null)
        {
            _characterActions = new CharacterActions();

            _characterActions.Player.Move.performed += input => _move = input.ReadValue<Vector2>();
            _characterActions.Player.Look.performed += input => _look = input.ReadValue<Vector2>();
            _characterActions.Player.Fire.performed += _ => _isFiring = true;
            _characterActions.Player.Fire.canceled += _ => _isFiring = false;
            _characterActions.Player.Aim.performed += _ => _isAiming = true;
            _characterActions.Player.Aim.canceled += _ => _isAiming = false;
        }
        
        _characterActions.Enable();
    }

    protected override void Update()
    {
        if (!IsAlive) return;

        if (_actionState != ActionState.IsAttacking && characterState == CharacterState.Equipped_OneHanded)
            _comboElapsedTime += Time.deltaTime;
        
        if (Health <= 0f) Die();
        
        HandleCamera();
        HandleMovement();
        HandleAim();
        HandleAttack();
    }
    
    private void HandleCamera()
    {
        _followTransform.position = new Vector3(transform.position.x, _followTransform.position.y, transform.position.z);

        // Rotate the follow target based on input
        _followTransform.rotation *= Quaternion.AngleAxis(_look.x * _sensitivity, Vector3.up);
        _followTransform.rotation *= Quaternion.AngleAxis(_look.y * _sensitivity, Vector3.right);

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


            if (!_isAiming)
            {
                Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
        }

        Vector2 movement = new Vector2(Mathf.Abs(_move.x), Math.Abs(_move.y));
        _animator.SetFloat("movement", movement.magnitude);
    }
    
    private void HandleAim()
    {
        _mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

        Ray ray = _mainCamera.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
            _debugSphere.position = raycastHit.point;
            _mouseWorldPosition = raycastHit.point;
        }
        
        if (_isAiming)
        {
            if (!_aimCamera.activeInHierarchy)
            {
                _moveCamera.SetActive(false);
                _aimCamera.SetActive(true);
            }
            _sensitivity = aimSensitivity;
            _animator.SetBool("IsAiming", true);

            Vector3 aimTarget = _mouseWorldPosition;
            aimTarget.y = transform.position.y;
            Vector3 aimDirection = (aimTarget - transform.position).normalized;
            
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else if (!_isAiming)
        {
            if (!_moveCamera.activeInHierarchy)
            {
                _moveCamera.SetActive(true);
                _aimCamera.SetActive(false);
            }
            _sensitivity = cameraSensitivity;
            _animator.SetBool("IsAiming", false);
        }
    }
    
    private void HandleAttack()
    {
        if (_actionState != ActionState.Unoccupied) return;
        
        // SWORD
        if (characterState == CharacterState.Equipped_OneHanded)
        {
            if (!_isFiring)
                _hasAttacked = false;
            
            if (_isFiring && !_hasAttacked)
            {
                _hasAttacked = true;
                AttackWithSword();
            }
        }
        
        // RIFLE
        else if (characterState == CharacterState.Equipped_Rifle)
        {
            if (!_rifleWeapon)
            {
                _rifleWeapon = _weapon.GetComponent<RifleWeapon>();
            }
            
            if (_isFiring) ShootRifle();
        }
    }

    private void AttackWithSword()
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

    private void ShootRifle()
    {
        if (_rifleWeapon)
        {
            _rifleWeapon.Shoot(_mouseWorldPosition);
            _animator.SetTrigger("Shoot");
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
