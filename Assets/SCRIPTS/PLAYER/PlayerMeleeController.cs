using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
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

    [SerializeField] private GameObject[] attackHitEffects;
    
    //Killstreaks
    private KillstreakManager _killStreaks;

    [Header("Audio")] //Source should be on the MeleeAttackPoint (Child of player->3dModel->armature->hips->spine->spine1
    [SerializeField] private AudioSource meleeAudioSource;
    [SerializeField] private AudioClip punchSoundClip;
    [SerializeField] private AudioClip hitSoundClip;

    private List<ITakeDamage> _hitEnemies = new List<ITakeDamage>();
    
    public float DamageBuff { get; set; } = 0;
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

        
        meleeAudioSource.pitch = Random.Range(.9f, 1.1f);
        meleeAudioSource.PlayOneShot(punchSoundClip);
    }
    
    private void AttackRaycast()
    {
        _hitEnemies.Clear(); //Clear the list at the start of each attack

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
                    // Check if the enemy has already been hit in the current attack
                    if (_hitEnemies.Contains(damageTaker))
                        continue; // Skip to the next ray/iteration
                        
                    _hitEnemies.Add(damageTaker); // Add the new hit enemy to the list

                    damageTaker.TakeDamage(AttackDamage());
                    Debug.Log("Hit " + hitInfo.transform.name);

                    //Moved to GameManager
                    //_killStreaks.GrantKillStreaks();
                }
            }
        }
    }

    private void HitTarget(Vector3 hitPos)
    {
        meleeAudioSource.pitch = 1.0f;
        meleeAudioSource.PlayOneShot(hitSoundClip);
        
        GameObject hitEffectObj = Instantiate(GetRandomAttackEffect(), hitPos, Quaternion.identity);
        Destroy(hitEffectObj, .75f); //How long until the hit effect is destroyed after being instantiated
    }

    private void ResetAttack()
    {
        isAttacking = false;
        isReadyToAttack = true;
    }

    public float AttackDamage()
    {
        float damage = Random.Range(minMeleeDamage, maxMeleeDamage);
        return damage + DamageBuff;
    }

    private GameObject GetRandomAttackEffect()
    {
        return attackHitEffects[Random.Range(0, attackHitEffects.Length)];
    }
}
