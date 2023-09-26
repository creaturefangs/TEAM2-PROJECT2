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
        Run,
        Shoot
    }

    public EnemyStates currentEnemyState;
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
    [SerializeField] private float currentSpeed = 2.0f;
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 2.0f;
    [SerializeField] private float speedWhileShooting = .5f;
    [SerializeField] private float distanceToPlayer; //How far away the player is
    
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
        
        //Find player on start
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (isAlive)
        {
            GetEnemyState();
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
            else if (distanceToPlayer <= canShootDistance && distanceToPlayer >= wanderDistance)
            {
                currentEnemyState = EnemyStates.Walk;
            }
            else
            {
                currentEnemyState = EnemyStates.Idle;
            }
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
                enemyAgent.speed = walkSpeed;

                if (!enemyAgent.pathPending && enemyAgent.remainingDistance <= .01f)
                {
                    int randomDestination = Random.Range(destinations[0, destinations.Length));
                }
                break;
            case EnemyStates.Run:
                enemyAgent.speed = runSpeed;
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
