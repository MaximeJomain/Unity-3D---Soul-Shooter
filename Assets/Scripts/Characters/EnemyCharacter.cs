using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyCharacter : Character
{
    #region Fields
    
    private EnemySpawner _enemySpawner;

    [SerializeField]
    private LayerMask groundLayer, playerLayer;
    private NavMeshAgent _agent;
    private Transform _player;
    private float _baseSpeed;

    // Patrolling
    [SerializeField]
    private float walkPointRange;
    private Vector3 _walkPoint;
    private bool _walkPointSet;

    // Attacking
    private const float attackCooldown = 3f;
    private bool _hasAttacked;

    // Ranges
    [SerializeField]
    private float sightRange, attackRange;
    private bool _playerInSightRange, _playerInAttackRange;
    

    #endregion
    
    protected override void Awake()
    {
        base.Awake();
        _player = GameObject.Find("Paladin Player").transform;
        _enemySpawner = GameObject.Find("Enemy Spawner").GetComponent<EnemySpawner>();
        _agent = GetComponent<NavMeshAgent>();
        
        GameObject weaponInstance = Instantiate(weaponPrefab.gameObject);
        weapon = weaponInstance.GetComponent<Weapon>();
        weaponAttackCollider = weaponInstance.GetComponent<Collider>();
        weapon.Equip(this);
    }

    protected override void Start()
    {
        base.Start();
        _agent.speed = MovementSpeed;
    }

    protected override void Update()
    {
        if (IsAlive)
        {
            if (_actionState == ActionState.IsAttacking)
            {
                _agent.speed = MovementSpeed * 0.1f;
            }
            else
            {
                _agent.speed = MovementSpeed * 2f;
            }
            
            float actualSpeed = _agent.velocity.magnitude;
            _animator.SetFloat("Speed", actualSpeed);
            
            if (Health <= 0f)
            {
                Die();
            }
            
            _playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
            _playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

            if (!_playerInSightRange && !_playerInAttackRange) ChasePlayer();
            if (_playerInSightRange && !_playerInAttackRange) ChasePlayer();
            if (_playerInSightRange && _playerInAttackRange) AttackPlayer();
        }
        else if (!IsAlive)
        {
            _agent.velocity = Vector3.zero;
        }
    }
    
    protected override void Die()
    {
        base.Die();
        _enemySpawner.AddKill();
        _rigidbody.velocity = Vector3.zero;
        
        Destroy(characterCollider);
        Destroy(gameObject, 5f);
    }

    private void Patrol()
    {
        if (!_walkPointSet) SearchWalkPoint();

        if (_walkPointSet)
            _agent.SetDestination(_walkPoint);

        Vector3 distanceToWalkPoint = transform.position - _walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            _walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        _agent.speed = MovementSpeed;

        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        _walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(_walkPoint, -transform.up, 2f, groundLayer))
            _walkPointSet = true;
    }

    private void ChasePlayer()
    {
        _agent.SetDestination(_player.position);
    }

    private void AttackPlayer()
    {
        transform.LookAt(_player);

        if (!_hasAttacked)
        {
            Attack();

            _hasAttacked = true;
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    private void ResetAttack()
    {
        _hasAttacked = false;
    }
}
