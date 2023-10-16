// C# 9.0 code
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class PlayerMeleeController : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackDistance = 3.0f;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Collider attackCollider;

    [SerializeField] private float attackDelay = .4f;
    [SerializeField] private float attackSpeed = 1.0f;
    [SerializeField] private float minMeleeDamage = 30.0f;
    [SerializeField] private float maxMeleeDamage = 75.0f;
   
    private bool isAttacking;
    private bool isReadyToAttack = true;

    [SerializeField] private GameObject[] attackHitEffects;

    [SerializeField] private AudioSource meleeAudioSource;
    [SerializeField] private AudioClip punchSoundClip;
    [SerializeField] private AudioClip hitSoundClip;

    private List<ITakeDamage> _hitEnemies = new List<ITakeDamage>();

    public float DamageBuff { get; set; } = 0;

    private void Awake() 
    {
        attackCollider.enabled = false;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && isReadyToAttack)
        {
            Attack();
        }
    }

    private void Attack()
    {
        isReadyToAttack = false;
        isAttacking = true;

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(EnableAttackCollider), attackDelay);

        meleeAudioSource.pitch = Random.Range(.9f, 1.1f);
        meleeAudioSource.PlayOneShot(punchSoundClip);
    }
    
    private void EnableAttackCollider()
    {
        attackCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & enemyMask) != 0) // if the other object is in the enemy layermask
        {
            ITakeDamage damageTaker = other.GetComponent<ITakeDamage>();
            
            // Check if the enemy has already been hit in the current attack
            if (!_hitEnemies.Contains(damageTaker))
            {
                _hitEnemies.Add(damageTaker); // Add the new hit enemy to the list
                damageTaker.TakeDamage(AttackDamage());
                HitTarget(other.transform.position);
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
        attackCollider.enabled = false;
        _hitEnemies.Clear(); //Clear the list at the end of each attack
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