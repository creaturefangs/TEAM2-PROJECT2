using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class EnemyController : MonoBehaviour, IFreeze
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
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private EnemyWeaponController weaponController;
    public Transform enemyForwardVector;

    [Header("AI Variables and Values")] 
    [SerializeField] private Transform[] destinations;
    private Transform _currentDestination;
    private int _currentPointIndex = 0;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float speedWhileShooting = .5f;
    [SerializeField] private float minIdleTime = 1f; //Minimum time the enemy will idle
    [SerializeField] private float maxIdleTime = 5f; //Maximum time the enemy will idle
    public float distanceToPlayer; //How far away the player is
    
    [Header("Combat")]
    public float canShootDistance = 20.0f;
    [SerializeField] private float wanderDistance = 15.0f;
    
    [Header("Freezing")]
    public bool isFrozen = false;
    
    [Header("Misc Variables")] 
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float maxPlayerDistance = 50.0f;
    public GameObject player;
    
    
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
        if (isAlive && !isFrozen)
        {
            GetEnemyState();
            HandleEnemyState();
        }
    }

    private void GetEnemyState()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        
        if (distanceToPlayer > canShootDistance && distanceToPlayer <= wanderDistance)
        {
            currentEnemyState = EnemyStates.Walk;
            enemyAgent.isStopped = false; // Let the agent move again
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
                if (!isFrozen) // Only set destination when not frozen
                {
                    if (!enemyAgent.pathPending && enemyAgent.remainingDistance <= .025f)
                    {
                        // If the enemy reaches the current destination, find the next one
                        FindNextDestination();
                        Debug.Log("Moving to next destination");
                    }
                }
                enemyAgent.speed = walkSpeed; // make the enemy walk at walk speed
                break;
            case EnemyStates.Shoot:
                if (IsPlayerInRange(canShootDistance))
                {
                    if (IsPlayerInLineOfSight()) // Check if the player is in the enemy's line of sight
                    {
                        weaponController.Shoot();

                        // Look at player
                        Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
                        transform.rotation = Quaternion.RotateTowards(
                            transform.rotation,
                            targetRotation,
                            weaponController.lookRotationSpeed * Time.deltaTime);
                    }
                    else
                    {
                        // Player is out of line of sight, switch to another state (e.g., Wander)
                        currentEnemyState = EnemyStates.Walk;
                    }
                }
                else
                {
                    // Player is out of range, switch to another state (e.g., Wander)
                    currentEnemyState = EnemyStates.Walk;
                }
                break;
        }

        currentSpeed = enemyAgent.speed;
        //TODO:
        //Set enemies' speed in the animator here
    }

    
    private void FindNextDestination()
    {
        int randomDestination = Random.Range(0, destinations.Length);
        _currentDestination = destinations[randomDestination];
        enemyAgent.SetDestination(_currentDestination.position);
    }


    private bool IsPlayerInLineOfSight()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, maxPlayerDistance, playerMask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    
    private IEnumerator SetIdleState(float idleTime)
    {
        currentEnemyState = EnemyStates.Idle;

        // Wait for the specified idle time
        yield return new WaitForSeconds(idleTime);

        // After idling, the enemy should go back to walking if the player is not in range
        if (!IsPlayerInRange(maxPlayerDistance))
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

    public bool IsPlayerInRange(float distanceVariable)
    {
        return distanceToPlayer <= distanceVariable;
    }

    public void ApplyFreeze(float duration, float slowSpeed, float slowAnimSpeed)
    {
        StartCoroutine(FreezeCoroutine(duration, slowSpeed, slowAnimSpeed));
    }

    private IEnumerator FreezeCoroutine(float freezeTime, float slowSpeed, float slowAnimSpeed)
    {
        if (!isFrozen) // Ensure enemy is only frozen once
        {
            isFrozen = true;
            float normalSpeed = enemyAgent.speed;
            float normalAnimSpeed = enemyAnimator.speed;

            enemyAgent.speed = slowSpeed;
            enemyAnimator.speed = slowAnimSpeed;

            yield return new WaitForSeconds(freezeTime);

            isFrozen = false;
            enemyAgent.speed = normalSpeed;
            enemyAnimator.speed = normalAnimSpeed;

            // After the freeze, check if the enemy should return to walking
            if (!IsPlayerInRange(wanderDistance))
            {
                currentEnemyState = EnemyStates.Walk;
            }
        }
    }


}
