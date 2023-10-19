using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class PlayerMeleeController : MonoBehaviour
{
    //[SerializeField] private Transform attackPoint;
    //[SerializeField] private float attackDistance = 3.0f;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Collider rightAttackCollider;
    [SerializeField] private Collider leftAttackCollider;

    [SerializeField] private float attackDelay = .4f;
    [SerializeField] private float attackSpeed = 1.0f;
    [SerializeField] private float minMeleeDamage = 30.0f;
    [SerializeField] private float maxMeleeDamage = 75.0f;

    [SerializeField] private float heavyAttackDelay = .6f;
    [SerializeField] private float heavyAttackSpeed = 2.0f;
    [SerializeField] private float minHeavyMeleeDamage = 50.0f;
    [SerializeField] private float maxHeavyMeleeDamage = 80.0f;

#pragma warning disable CS0414 // Field is assigned but its value is never used
    private bool isAttacking;
#pragma warning restore CS0414 // Field is assigned but its value is never used
    private bool isHeavyAttacking;
    private bool isReadyToAttack = true;

    [SerializeField] private GameObject[] attackHitEffects;

    [SerializeField] private AudioSource meleeAudioSource;
    [SerializeField] private AudioClip punchSoundClip;
    [SerializeField] private AudioClip hitSoundClip;

    private List<ITakeDamage> _hitEnemies = new List<ITakeDamage>();

    public float DamageBuff { get; set; } = 0;

    private void Awake() 
    {
        rightAttackCollider.enabled = false;
        leftAttackCollider.enabled = false;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && isReadyToAttack)
        {
            isAttacking = true;
            Attack(rightAttackCollider, attackSpeed, attackDelay);
        }
        else if (Mouse.current.rightButton.wasPressedThisFrame && isReadyToAttack)
        {
            isHeavyAttacking = true;
            Attack(leftAttackCollider, heavyAttackSpeed, heavyAttackDelay);
        }
    }

    private void Attack(Collider attackCollider, float speed, float delay)
    {
        isReadyToAttack = false;

        Invoke(nameof(ResetAttack), speed);
        StartCoroutine(EnableAttackCollider(attackCollider, delay));
    
        // Play the punch sound after the attack delay
        Invoke(nameof(PlayPunchSound), delay - delay * .5f);
    }
    
    private void PlayPunchSound()
    {
        meleeAudioSource.pitch = Random.Range(.9f, 1.1f);
        meleeAudioSource.PlayOneShot(punchSoundClip);
    }

    private IEnumerator EnableAttackCollider(Collider attackCollider, float delay)
    {
        yield return new WaitForSeconds(delay);
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
                damageTaker.TakeDamage(AttackDamage(isHeavyAttacking));
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
        isHeavyAttacking = false;
        isReadyToAttack = true;
        rightAttackCollider.enabled = false;
        leftAttackCollider.enabled = false;
        _hitEnemies.Clear(); //Clear the list at the end of each attack
    }

    public float AttackDamage(bool isHeavyAttack)
    {
        float minDamage = isHeavyAttack ? minHeavyMeleeDamage : minMeleeDamage;
        float maxDamage = isHeavyAttack ? maxHeavyMeleeDamage : maxMeleeDamage;
        float damage = Random.Range(minDamage, maxDamage);
        return damage + DamageBuff;
    }
    
    private GameObject GetRandomAttackEffect()
    {
        return attackHitEffects[Random.Range(0, attackHitEffects.Length)];
    }
}