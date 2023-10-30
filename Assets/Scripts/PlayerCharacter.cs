using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerCharacter : Character
{
    [SerializeField]
    private float cameraSensitivity = 3f;

    [SerializeField]
    private float rotationSpeed = 5f;

    [SerializeField]
    private Transform followTransform;

    private Vector2 _move;

    private Vector2 _look;


    protected override void Update()
    {
        if (!IsAlive) return;
        
        if (Health <= 0f)
        {
            Die();
        }

        HandleCamera();

        HandleMovement();
    }

    private void HandleCamera()
    {
        followTransform.position = new Vector3(transform.position.x, followTransform.position.y, transform.position.z);

        // Rotate the follow target based on input
        followTransform.rotation *= Quaternion.AngleAxis(_look.x * cameraSensitivity, Vector3.up);
        followTransform.rotation *= Quaternion.AngleAxis(_look.y * cameraSensitivity, Vector3.right);

        var localAngles = followTransform.localEulerAngles;
        localAngles.z = 0f;

        var localAngleX = followTransform.localEulerAngles.x;

        if (localAngleX > 180 && localAngleX < 340)
        {
            localAngles.x = 340;
        }
        else if (localAngleX < 180 && localAngleX > 40)
        {
            localAngles.x = 40f;
        }

        followTransform.transform.localEulerAngles = localAngles;
    }

    private void HandleMovement()
    {
        if (_actionState != ActionState.Unoccupied) return;
        
        if (_move != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, followTransform.rotation.eulerAngles.y, 0f);

            Vector3 moveDirection = targetRotation * new Vector3(_move.x, 0f, _move.y);
            _rigidbody.velocity = moveDirection * MovementSpeed;

            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        _animator.SetFloat("moveX", Mathf.Abs(_move.x));
        _animator.SetFloat("moveY", Mathf.Abs(_move.y));
    }

    private void OnMove(InputValue value)
    {
        _move = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        _look = value.Get<Vector2>();
    }

    private void OnFire()
    {
        if (_characterState == CharacterState.Equipped_OneHanded
            && _actionState == ActionState.Unoccupied)
        {
            _actionState = ActionState.IsAttacking;
            int randomAttack = Random.Range(1, 2);
            _animator.SetTrigger("Attack" + randomAttack);
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
