using System;
using Invector.vCharacterController;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, ITakeDamage
{
    public float currentHealth;
    private float _maxHealth = 100;
    public bool isAlive = true;

    private EnemyController enemyController;

    private vRagdoll ragdoll;
    private void Start()
    {
        currentHealth = _maxHealth;
        enemyController = GetComponent<EnemyController>();
        ragdoll = GetComponent<vRagdoll>();
    }

    public void ApplyDamage(float amount)
    {
        currentHealth -= amount;
    }

    public void TakeDamage(float amount)
    {
        ApplyDamage(amount);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isAlive = false;
        enemyController.enemyAgent.speed = 0.0f;
        
        transform.root.SendMessage ("ActivateRagdoll", SendMessageOptions.DontRequireReceiver);
    }
}
