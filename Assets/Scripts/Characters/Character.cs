using JetBrains.Annotations;
using UnityEngine;

public enum CharacterState
{
    Unequipped,
    Equipped_OneHandedSword,
    Equipped_Rifle,
    Equipped_HandGun,
}

public enum ActionState
{
    Unoccupied,
    IsAttacking,
    IsDodging,
}

public class Character : MonoBehaviour
{

    #region Class Fields

    [SerializeField]
    public float Health;
    [SerializeField]
    public float MovementSpeed;
    [HideInInspector]
    public CharacterState CharacterState = CharacterState.Unequipped;
    [SerializeField]
    public Transform SwordSocket, HandgunSocket, RifleSocket;
    
    protected bool IsAlive { get; private set; }
    [SerializeField]
    protected Weapon weaponPrefab;
    protected bool _isInvincible = false;
    protected Rigidbody _rigidbody;
    protected Animator _animator;
    protected Weapon weapon;
    protected ActionState _actionState = ActionState.Unoccupied;
    protected Collider weaponAttackCollider;
    protected CapsuleCollider characterCollider;

    #endregion

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        characterCollider = GetComponent<CapsuleCollider>();
    }

    protected virtual void Start()
    {
        IsAlive = true;
    }

    protected virtual void Update()
    {
        if (IsAlive)
        {
            if (Health <= 0f)
            {
                Die();
            }
        }
    }

    protected void Attack()
    {
        _animator.SetTrigger("Attack");
    }

    public void AttackStart()
    {
        weaponAttackCollider.enabled = true;
    }

    public void AttackEnd()
    {
        weaponAttackCollider.enabled = false;
    }

    public void SetActionState(ActionState actionState)
    {
        _actionState = actionState;
    }

    public void TakeDamage(float damage)
    {
        if (_isInvincible) return;
        Health -= damage;
    }

    protected virtual void Die()
    {
        IsAlive = false;
        _animator.SetTrigger("Death");
    }
}
