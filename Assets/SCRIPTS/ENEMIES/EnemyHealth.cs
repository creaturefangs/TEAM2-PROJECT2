using System;
using System.Collections;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemyHealth : MonoBehaviour, ITakeDamage
{
    public float currentHealth;
    private float _maxHealth = 100;
    public bool isAlive = true;

    private bool _canTakeDamage = true;
    private EnemyController enemyController;
    private vRagdoll ragdoll;

    [SerializeField] private GameObject[] bloodPoolPrefabs;
    
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
        if (_canTakeDamage)
        {
            ApplyDamage(amount);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        _canTakeDamage = false;
        isAlive = false;
        enemyController.enemyAgent.speed = 0.0f;
        
        GameManager.instance.IncrementPlayerKillCount();
        PlayerUI.instance.UpdateKillsUI(GameManager.instance.killCounter,
            GameManager.instance.killsToNextKillStreak);
        transform.root.SendMessage ("ActivateRagdoll", SendMessageOptions.DontRequireReceiver);

        StartCoroutine(BloodPoolAfterDeath());
    }

    private IEnumerator BloodPoolAfterDeath()
    {
        yield return new WaitForSeconds(2.0f);

        GameObject blood = GetRandomBloodPrefab();
        Instantiate(blood, enemyController.feetPosition.position, blood.transform.rotation);

       // Destroy(blood, 10.0f);
    }

    private GameObject GetRandomBloodPrefab()
    {
        return bloodPoolPrefabs[Random.Range(0, bloodPoolPrefabs.Length)];
    }
}
