using UnityEngine;

public enum CharacterState
{
    Unequipped,
    Equipped_OneHanded,
}
public enum ActionState
{
    Unoccupied,
    IsAttacking,
}

public class Character : MonoBehaviour
{
    [SerializeField]
    public float Health;
    
    [SerializeField]
    public float MovementSpeed;
    
    [SerializeField]
    private Weapon weapon;
    
    [SerializeField]
    private Transform _handSocket;

    protected bool IsAlive { get; private set; }
    
    protected Rigidbody _rigidbody;
    
    protected Animator _animator;

    protected CharacterState _characterState = CharacterState.Unequipped;

    protected ActionState _actionState = ActionState.Unoccupied;

    private Collider _weaponAttackCollider;
    
    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        
        GameObject weaponInstance = Instantiate(weapon.gameObject, _handSocket);
        weaponInstance.transform.position = _handSocket.position;
        weaponInstance.transform.rotation = _handSocket.rotation;
        
        // TODO Remove with equipping weapon action
        _characterState = CharacterState.Equipped_OneHanded;
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
        _animator.SetTrigger("attack");
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
        Health -= damage;
    }

    protected virtual void Die()
    {
        IsAlive = false;
        _animator.SetTrigger("death");
    }
}
