using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public enum CharacterState
{
    Unequipped,
    Equipped_OneHanded,
    Equipped_Rifle,
}

public enum ActionState
{
    Unoccupied,
    IsAttacking,
    IsDodging,
}

public class Character : MonoBehaviour {
    [SerializeField]
    public float Health;
    
    [SerializeField]
    public float MovementSpeed;
    
    [FormerlySerializedAs("weapon")]
    [SerializeField]
    protected Weapon weaponPrefab;
    
    [SerializeField]
    public Transform HandSocket;

    [SerializeField]
    [CanBeNull]
    public Transform RifleHandSocket;

    protected bool IsAlive { get; private set; }
    
    protected bool _isInvincible = false;
    
    protected Rigidbody _rigidbody;
    
    protected Animator _animator;

    protected GameObject _weapon;

    [HideInInspector]
    public CharacterState characterState = CharacterState.Unequipped;

    protected ActionState _actionState = ActionState.Unoccupied;

    private Collider _weaponAttackCollider;
    
    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        
        _weapon = Instantiate(weaponPrefab.gameObject, HandSocket);
        _weapon.GetComponent<Weapon>().Equip(this);
        
        _weaponAttackCollider = _weapon.GetComponent<Collider>();
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
