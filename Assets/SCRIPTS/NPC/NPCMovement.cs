using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NPCMovement : MonoBehaviour
{
    private enum NPCStates
    {
        Idle,
        Walk,
        Flee
    }

    private NPCStates currentNPCState;
    

    [Header("AI")] 
    private NavMeshAgent npcAgent;
    [SerializeField] private Transform[] destinations;
    [SerializeField] private float walkSpeed = 2.5f;
    [SerializeField] private float idleTimer = 3.0f;
    [SerializeField] private Transform fleeDestination;
    private bool _idling = false;
    private bool _canIdle = true;

    [Header("Other Components")] 
    private Animator npcAnimator;
    private void Awake()
    {
        npcAgent = GetComponent<NavMeshAgent>();
        npcAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentNPCState = NPCStates.Walk;
        npcAgent.SetDestination(GetRandomDestination().position);
    }

    private void Update()
    {
        if (!_idling)
        {
            Move();
        }
    }


    
    private void Move()
    {
        switch (currentNPCState)
        {
            case NPCStates.Walk:
            if (!npcAgent.pathPending && npcAgent.remainingDistance <= .01f)
            {
                StartCoroutine(IdleRoutine());
            }

            npcAgent.isStopped = false;
            npcAgent.speed = walkSpeed;
            break;
            default: Debug.Log("Error in NPCMovement/Move.");
                break;
        }
    }

    private void SetNpcAnimationState(NPCStates state)
    {
        switch (state)
        {
            case NPCStates.Idle:
                npcAnimator.SetBool("Idle", true);
                npcAnimator.SetBool("Walking", false);
                npcAnimator.SetBool("Fleeing", false);
                break;
            case NPCStates.Walk:
                npcAnimator.SetBool("Idle", false);
                npcAnimator.SetBool("Walking", true);
                npcAnimator.SetBool("Fleeing", false);
                break;
            case NPCStates.Flee:
                npcAnimator.SetBool("Fleeing", true);
                npcAnimator.SetBool("Walking", false);
                npcAnimator.SetBool("Idle", false);
                break;
        }
    }

    private IEnumerator IdleRoutine()
    {
        if (_canIdle)
        {
            npcAgent.speed = 0.0f;
            SetNpcAnimationState(NPCStates.Idle);
            currentNPCState = NPCStates.Idle;
            _idling = true;
            yield return new WaitForSeconds(idleTimer);
        
            npcAgent.speed = walkSpeed;
            npcAgent.SetDestination(GetRandomDestination().position);
            SetNpcAnimationState(NPCStates.Walk);
            currentNPCState = NPCStates.Walk;
            _idling = false;
        }
    }

    private Transform GetRandomDestination()
    {
        return destinations[Random.Range(0, destinations.Length)];
    }

    private void ClearDestinations()
    {
        for (int i = 0; i < destinations.Length; i++)
        {
            destinations[i] = null;
        }
    }

    public void FleeFromGunFire()
    {
        currentNPCState = NPCStates.Flee;
        SetNpcAnimationState(currentNPCState);
        _canIdle = false;
        ClearDestinations();
        npcAgent.SetDestination(fleeDestination.position);

        if (!npcAgent.pathPending && npcAgent.remainingDistance <= .01f)
        {
            Destroy(gameObject);
        }
    }
}
