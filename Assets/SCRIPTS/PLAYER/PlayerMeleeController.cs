using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerMeleeController : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackDistance = 3.0f;
    [SerializeField] private float attackDelay = .4f;
    [SerializeField] private float attackSpeed = 1.0f;

    [SerializeField] private float minMeleeDamage = 30.0f;
    [SerializeField] private float maxMeleeDamage = 75.0f;
   
    [SerializeField] private LayerMask enemyMask;

    private bool isAttacking;
    private bool isReadyToAttack = true;

    [SerializeField] private GameObject attackHitEffect;
    
    //Killstreaks
    private KillstreakManager _killStreaks;

    [Header("Audio")] //Source should be on the MeleeAttackPoint (Child of player->3dModel->armature->hips->spine->spine1
    [SerializeField] private AudioSource meleeAudioSource;
    [SerializeField] private AudioClip punchSound;
    [SerializeField] private AudioClip hitSound;

    private void Awake()
    {
        _killStreaks = GetComponent<KillstreakManager>();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (!isReadyToAttack || isAttacking) return;

        isReadyToAttack = false;
        isAttacking = true;

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);

        
        //meleeAudioSource.pitch = Random.Range(.9f, 1.1f);
        //meleeAudioSource.PlayOneShot(punchSound);
    }

    // private void AttackRaycast()
    // {
    //     Debug.Log("AttackRaycast");
    //     if (Physics.Raycast(
    //             attackPoint.transform.position,
    //             attackPoint.transform.forward,
    //             out RaycastHit hitInfo,
    //             attackDistance,
    //             enemyMask))
    //     {
    //         HitTarget(hitInfo.point);
    //
    //         if (hitInfo.collider.CompareTag("Enemy") && hitInfo.transform.TryGetComponent(out ITakeDamage damageTaker))
    //         {
    //             damageTaker.TakeDamage(AttackDamage());
    //             Debug.Log("Hit " + hitInfo.transform.name);
    //             
    //             _killStreaks.GrantKillStreaks();
    //         }
    //     }
    // }
    
    private void AttackRaycast()
    {
        int rayCount = 5; // how many rays to cast out from the attack point
        float spreadAngle = 45f; // total raycast spread angle

        for (int i = 0; i < rayCount; i++)
        {
            float angle = spreadAngle * ((float)i / (rayCount - 1)) - spreadAngle / 2;

            Quaternion rotation = Quaternion.AngleAxis(angle, attackPoint.up);

            Vector3 direction = rotation * attackPoint.forward;

            if (Physics.Raycast(attackPoint.position, direction, out RaycastHit hitInfo, attackDistance, enemyMask))
            {
                HitTarget(hitInfo.point);

                if (hitInfo.collider.CompareTag("Enemy") && hitInfo.transform.TryGetComponent(out ITakeDamage damageTaker))
                {
                    damageTaker.TakeDamage(AttackDamage());
                    Debug.Log("Hit " + hitInfo.transform.name);
                
                    _killStreaks.GrantKillStreaks();
                }
            }
        }
    }

    private void HitTarget(Vector3 hitPos)
    {
       // meleeAudioSource.pitch = 1.0f;
       // meleeAudioSource.PlayOneShot(hitSound);
        
        //GameObject hitEffectObj = Instantiate(attackHitEffect, hitPos, Quaternion.identity);
       // Destroy(hitEffectObj, .75f); //How long until the hit effect is destroyed after being instantiated
    }

    private void ResetAttack()
    {
        isAttacking = false;
        isReadyToAttack = true;
    }

    public float AttackDamage()
    {
        return Random.Range(minMeleeDamage, maxMeleeDamage);
    }
}
