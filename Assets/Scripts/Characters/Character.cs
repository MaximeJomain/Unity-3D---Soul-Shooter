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
    
    [SerializeField]
    protected Weapon weapon;
    
    [SerializeField]
    public Transform HandSocket;

    protected bool IsAlive { get; private set; }
    
    protected bool _isInvincible = false;
    
    protected Rigidbody _rigidbody;
    
    protected Animator _animator;

    [HideInInspector]
    public CharacterState CharacterState = CharacterState.Unequipped;

    protected ActionState _actionState = ActionState.Unoccupied;

    private Collider _weaponAttackCollider;
    
    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        
        GameObject weaponInstance = Instantiate(weapon.gameObject, HandSocket);
        weaponInstance.GetComponent<Weapon>().Equip(this);
        
        _weaponAttackCollider = weaponInstance.GetComponent<Collider>();
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
