using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeController : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackDistance = 3.0f;
    [SerializeField] private float attackDelay = .4f;
    [SerializeField] private float attackSpeed = 1.0f;

    [SerializeField] private float minMeleeDamage = 30.0f;
    [SerializeField] private float maxMeleeDamage = 75.0f;
   
    [SerializeField] private LayerMask enemyMask;

    private bool _isAttacking = false;
    private bool _readyToAttack = true;

    [SerializeField] private GameObject attackHitEffect;

    [Header("Audio")] //Source should be on the MeleeAttackPoint (Child of player->3dmodel->armature->hips->spine->spine1
    [SerializeField] private AudioSource meleeAudioSource;
    [SerializeField] private AudioClip punchSound;
    [SerializeField] private AudioClip hitSound;
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (!_readyToAttack || _isAttacking) return;

        _readyToAttack = false;
        _isAttacking = true;

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);

        
        //meleeAudioSource.pitch = Random.Range(.9f, 1.1f);
        //meleeAudioSource.PlayOneShot(punchSound);
    }

    private void AttackRaycast()
    {
        Debug.Log("AttackRaycast");
        if (Physics.Raycast(
                attackPoint.transform.position,
                attackPoint.transform.forward,
                out RaycastHit hitInfo,
                attackDistance,
                enemyMask))
        {
            HitTarget(hitInfo.point);

            if (hitInfo.collider.CompareTag("Enemy") && hitInfo.transform.TryGetComponent(out ITakeDamage damageTaker))
            {
                damageTaker.TakeDamage(AttackDamage());
                Debug.Log("Hit " + hitInfo.transform.name);
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
        _isAttacking = false;
        _readyToAttack = true;
    }

    private float AttackDamage()
    {
        return Random.Range(minMeleeDamage, maxMeleeDamage);
    }
}
