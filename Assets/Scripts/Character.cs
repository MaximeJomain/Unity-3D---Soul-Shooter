using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Character : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 3f;

    [SerializeField]
    private float cameraSensitivity = 3f;

    [SerializeField]
    private float rotationSpeed = 5f;

    [SerializeField]
    private Transform followTransform;

    private Rigidbody _rigidbody;

    private Vector2 _move;

    private Vector2 _look;
    
    private Animator _animator;

    private bool _isAttacking;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        cameraSensitivity /= 10;
    }

    private void Update()
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

        // Move the player

        if (_move != Vector2.zero)
        {
            if (_isAttacking) return;
            
            Quaternion targetRotation = Quaternion.Euler(0f, followTransform.rotation.eulerAngles.y, 0f);

            Vector3 moveDirection = targetRotation * new Vector3(_move.x, 0f, _move.y);
            _rigidbody.velocity = moveDirection * movementSpeed;

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
        if (_isAttacking == false)
        {
            _isAttacking = true;
            int randomAttack = Random.Range(1, 2);
            _animator.SetTrigger("attack" + randomAttack);
        }
    }

    public void SetIsAttacking(int value)
    {
        _isAttacking = value != 0;
    }
}
