using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class EnemyController : MonoBehaviour, IFreeze
{
    public enum EnemyStates
    {
        Walk,
        Shoot
    }

    private EnemyStates currentEnemyState;
    private EnemyStates previousEnemyState;
    public EnemySO enemy;
    
    //Health Variables
    public float currentHealth { get; private set; }
    
    [Header("AI Components")]
    public NavMeshAgent enemyAgent;
    [SerializeField] private EnemyWeaponController weaponController;
    public Transform enemyForwardVector;
    public Transform feetPosition;
    private EnemyHealth enemyHealth;

    [Header("AI Variables and Values")] 
    [SerializeField] private Transform[] destinations;
    private Transform _currentDestination;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float speedWhileShooting = .5f;

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


    [Header("Animation")]
    public Animator enemyAnimator;
    private void Start()
    {
        currentHealth = enemy.maxHealth;

        currentEnemyState = EnemyStates.Walk;
    
        // Ensure that the agent is in the moving state
        enemyAgent.isStopped = false;

        _currentDestination = GetRandomDestination();
        enemyAgent.SetDestination(_currentDestination.position);
    
        // Find player on start
        player = GameObject.FindGameObjectWithTag("Player");

        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Update()
    {
        if (enemyHealth.isAlive && !isFrozen)
        {
            GetEnemyState();
            HandleEnemyState();

            if (currentEnemyState == EnemyStates.Shoot && IsPlayerInLineOfSight())
            {
                weaponController.Shoot();
            }
        }
    }

    private void GetEnemyState()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= canShootDistance) 
        {
            currentEnemyState = EnemyStates.Shoot;
        }
        else 
        {
            currentEnemyState = EnemyStates.Walk;
        }
    }

    private void HandleEnemyState()
    {
        switch (currentEnemyState)
        {
            case EnemyStates.Walk:
                WalkStateActions();
                //FaceDestination(true, _currentDestination);
                break;
            case EnemyStates.Shoot:
                ShootStateActions();
                //FaceDestination(false, _currentDestination);
                break;
        }
        currentSpeed = enemyAgent.speed;
        //TODO: Set enemies' speed in the animator here
        SetEnemyAnimationState(currentEnemyState);
    }

    private void SetEnemyAnimationState(EnemyStates state)
    {
        if (!enemyAnimator.GetBool("isDead"))
        {
            switch (state)
            {
                case EnemyStates.Walk:
                    enemyAnimator.SetBool("isWalking", false);
                    enemyAnimator.SetBool("isRunning", true);
                    break;
                case EnemyStates.Shoot:
                    enemyAnimator.SetBool("isWalking", true);
                    enemyAnimator.SetBool("isRunning", false);
                    break;
            }
        }
    }

    private void FaceDestination(bool walking, Transform destination)
    {
        if (!walking)
        {
            Vector3 targetDirection = (destination.position - transform.position).normalized;

            Quaternion lookDirection = Quaternion.LookRotation(targetDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection,
                Time.deltaTime * weaponController.lookRotationSpeed);
        }
       
    }
    
    private void WalkStateActions()
    {
        if (!isFrozen) 
        {
            if (!enemyAgent.pathPending && enemyAgent.remainingDistance <= .025f)
            {
                // If the enemy reaches the current destination, find the next one
                FindNextDestination();
            }
        }
        // Only set destination when not frozen
        enemyAgent.isStopped = false; 
        enemyAgent.speed = walkSpeed; 
    }

    private void ShootStateActions()
    {
        enemyAgent.speed = speedWhileShooting;
    }
    
    private void FindNextDestination()
    {
        int randomDestination = Random.Range(0, destinations.Length);
        _currentDestination = destinations[randomDestination];
    
        // Ensure that the agent is in the moving state
        enemyAgent.isStopped = false;

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
    
    private Transform GetRandomDestination()
    {
        return destinations[Random.Range(0, destinations.Length)];
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
