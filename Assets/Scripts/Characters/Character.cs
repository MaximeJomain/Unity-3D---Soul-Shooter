using JetBrains.Annotations;
using UnityEngine;

public enum CharacterState
{
    Unequipped,
    Equipped_OneHanded,
    Equipped_Rifle,
    Equipped_HandGun,
}

public enum ActionState
{
    Unoccupied,
    IsAttacking,
    IsDodging,
}

public class Character : MonoBehaviour {

    #region Class Fields
    
    [SerializeField]
    public float Health;
    
    [SerializeField]
    public float MovementSpeed;
    
    [SerializeField]
    protected Weapon weaponPrefab;
    
    [SerializeField]
    public Transform HandSocket;

    [SerializeField]
    [CanBeNull]
    public Transform RifleHandSocket;
    
    [HideInInspector]
    public CharacterState characterState = CharacterState.Unequipped;

    protected bool IsAlive { get; private set; }
    protected bool _isInvincible = false;
    protected Rigidbody _rigidbody;
    protected Animator _animator;
    protected Weapon _weapon;
    protected ActionState _actionState = ActionState.Unoccupied;
    private Collider _weaponAttackCollider;
    protected CapsuleCollider characterCollider;
    
    #endregion
    
    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        characterCollider = GetComponent<CapsuleCollider>();
        
        GameObject weaponInstance = Instantiate(weaponPrefab.gameObject, HandSocket);
        _weapon = weaponInstance.GetComponent<Weapon>();
        _weaponAttackCollider = weaponInstance.GetComponent<Collider>();
        
        _weapon.Equip(this);
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
        _weaponAttackCollider.enabled = true;
    }

    public void AttackEnd()
    {
        _weaponAttackCollider.enabled = false;
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
