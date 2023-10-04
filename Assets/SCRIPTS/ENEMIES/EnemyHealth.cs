using System;
using System.Collections;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, ITakeDamage
{
    public float currentHealth;
    private float _maxHealth = 100;
    public bool isAlive = true;
    
    private EnemyController enemyController;
    public UnityEvent deathEvent;
    private vRagdoll ragdoll;

    [SerializeField] private GameObject bloodPoolPrefab;
    
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
        deathEvent.Invoke();
        
        GameManager.instance.IncrementPlayerKillCount();
        PlayerUI.instance.UpdateKillsUI(GameManager.instance.killCounter,
            GameManager.instance.killsToNextKillStreak);
        transform.root.SendMessage ("ActivateRagdoll", SendMessageOptions.DontRequireReceiver);

        StartCoroutine(BloodPoolAfterDeath());
    }

    private IEnumerator BloodPoolAfterDeath()
    {
        yield return new WaitForSeconds(2.0f);

        GameObject blood = Instantiate(bloodPoolPrefab, enemyController.feetPosition.position, Quaternion.identity);

        Destroy(blood, 10.0f);
    }
}
