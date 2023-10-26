using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private LayerMask groundLayer, playerLayer;
    
    private NavMeshAgent _agent;

    private Transform _player;
    
    private Enemy _enemyController;
    
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
    

    private void Awake()
    {
        // TODO Automatic way to find Player
        _player = GameObject.Find("Paladin Player").transform;

        _agent = GetComponent<NavMeshAgent>();
        _enemyController = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (_enemyController.IsAlive)
        {
            _enemyController.Speed = _agent.velocity.magnitude;
            
            _playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
            _playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

            if (!_playerInSightRange && !_playerInAttackRange) Patrol();
            if (_playerInSightRange && !_playerInAttackRange) ChasePlayer();
            if (_playerInSightRange && _playerInAttackRange) AttackPlayer();
        }
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
        _agent.speed *= 1;
        
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        _walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(_walkPoint, -transform.up, 2f, groundLayer))
            _walkPointSet = true;
    }

    private void ChasePlayer()
    {
        _agent.speed *= 1.5f;
        _agent.SetDestination(_player.position);
    }
    
    private void AttackPlayer()
    {
        _agent.SetDestination(transform.position);
        
        transform.LookAt(_player);

        if (!_hasAttacked)
        {
            _enemyController.Attack();
            
            _hasAttacked = true;
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    private void ResetAttack()
    {
        _hasAttacked = false;
    }

    
}
