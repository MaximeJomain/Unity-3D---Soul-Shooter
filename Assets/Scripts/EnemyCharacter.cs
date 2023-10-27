using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyCharacter : Character
{
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
    [SerializeField]
    private float attackCooldown;
    private bool _hasAttacked;

    // Ranges
    [SerializeField]
    private float sightRange, attackRange;
    private bool _playerInSightRange, _playerInAttackRange;


    protected override void Awake()
    {
        base.Awake();
        // TODO Automatic way to find Player
        _player = GameObject.Find("Paladin Player").transform;

        _agent = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();
        _baseSpeed = _agent.speed;
    }

    protected override void Update()
    {
        if (IsAlive)
        {
            if (Health <= 0f)
            {
                Die();
            }
            
            MovementSpeed = _agent.velocity.magnitude;

            _playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
            _playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

            if (!_playerInSightRange && !_playerInAttackRange) ChasePlayer();
            if (_playerInSightRange && !_playerInAttackRange) ChasePlayer();
            if (_playerInSightRange && _playerInAttackRange) AttackPlayer();
        }
    }
    
    protected override void Die()
    {
        base.Die();
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
        _agent.speed = _baseSpeed;

        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        _walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(_walkPoint, -transform.up, 2f, groundLayer))
            _walkPointSet = true;
    }

    private void ChasePlayer()
    {
        _agent.speed = _baseSpeed * 2f;
        _agent.SetDestination(_player.position);
    }

    private void AttackPlayer()
    {
        _agent.SetDestination(transform.position);

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
