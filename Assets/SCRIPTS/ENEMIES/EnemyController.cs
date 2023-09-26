using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class EnemyController : MonoBehaviour, IFreeze, ITakeDamage
{
    public enum EnemyStates
    {
        Idle,
        Walk,
        Shoot
    }

    private EnemyStates currentEnemyState;
    private EnemyStates previousEnemyState;
    public EnemySO enemy;
    
    //Health Variables
    public float currentHealth { get; private set; }
    public bool isAlive = true;
    
    [Header("AI Components")]
    [SerializeField] private NavMeshAgent enemyAgent;
    [SerializeField] private Transform enemyForwardVector;
    [SerializeField] private Animator enemyAnimator;


    [Header("AI Variables and Values")] 
    [SerializeField] private Transform[] destinations;
    private Transform _currentDestination;
    private int _currentPointIndex = 0;
    [SerializeField] private float currentSpeed = 2.0f;
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float speedWhileShooting = .5f;
    [SerializeField] private float distanceToPlayer; //How far away the player is
    [SerializeField] private float minIdleTime = 1f; //Minimum time the enemy will idle
    [SerializeField] private float maxIdleTime = 5f; //Maximum time the enemy will idle
    
    [Header("Combat")] 
    private bool _canShoot = true;
    [SerializeField] private float canShootDistance = 20.0f;
    [SerializeField] private float wanderDistance = 15.0f;
    
    [Header("Misc Variables")] 
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float maxPlayerDistance = 50.0f;
    private GameObject player;
    
    
    private void Start()
    {
        currentHealth = enemy.maxHealth;

        currentEnemyState = EnemyStates.Walk;
        enemyAgent.SetDestination(GetRandomDestination().position);
        
        //Find player on start
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (isAlive)
        {
            previousEnemyState = currentEnemyState;
            GetEnemyState();
        
            // If it just transitions from Idle to Walk
            if (previousEnemyState == EnemyStates.Idle && currentEnemyState == EnemyStates.Walk)
            {
                enemyAgent.SetDestination(GetRandomDestination().position);
            }

            HandleEnemyState();
        }
    }

    private void GetEnemyState()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (IsPlayerInRange())
        {
            if (distanceToPlayer <= canShootDistance)
            {
                currentEnemyState = EnemyStates.Shoot;
            }
            else if (distanceToPlayer > canShootDistance && distanceToPlayer <= wanderDistance)
            {
                currentEnemyState = EnemyStates.Walk;
            }
        }
        else
        {
            StartCoroutine(SetIdleState(Random.Range(minIdleTime, maxIdleTime)));
        }
    }

    private void HandleEnemyState()
    {
        switch (currentEnemyState)
        {
            case EnemyStates.Idle: 
                enemyAgent.speed = 0.0f;
                break;
            case EnemyStates.Walk:
                if (!enemyAgent.pathPending && enemyAgent.remainingDistance <= .1f)
                {
                    int randomDestination = Random.Range(0, destinations.Length);
                    _currentDestination = destinations[randomDestination];
                    enemyAgent.SetDestination(_currentDestination.position);
                }
                break;
            case EnemyStates.Shoot:
                enemyAgent.speed = speedWhileShooting;

                if (_canShoot && IsLookingAtPlayer())
                {
                    Shoot();
                }
                break;
        }

        currentSpeed = enemyAgent.speed;
        //enemyAnimator.SetFloat("Speed", currentSpeed);
    }

    private IEnumerator SetIdleState(float idleTime)
    {
        currentEnemyState = EnemyStates.Idle;

        // Wait for the specified idle time
        yield return new WaitForSeconds(idleTime);

        // After idling, the enemy should go back to walking if the player is not in range
        if (!IsPlayerInRange())
        {
            currentEnemyState = EnemyStates.Walk;
        }
    }
    
    private Transform GetRandomDestination()
    {
        return destinations[Random.Range(0, destinations.Length)];
    }
    
    private bool IsLookingAtPlayer()
    {
        if (Physics.Raycast(
                enemyForwardVector.position,
                enemyForwardVector.transform.forward,
                out RaycastHit hitInfo,
                maxPlayerDistance,
                playerMask))
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsPlayerInRange()
    {
        return distanceToPlayer <= maxPlayerDistance;
    }

    private void Shoot()
    {
        
    }

    public void StartFreeze()
    {
        throw new NotImplementedException();
    }

    public void ApplyDamage()
    {
        throw new NotImplementedException();
    }

    public void TakeDamage()
    {
        throw new NotImplementedException();
    }

    public void Die()
    {
        throw new NotImplementedException();
    }
}
