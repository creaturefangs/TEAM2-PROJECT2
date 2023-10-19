using System;
using System.Collections;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemyHealth : MonoBehaviour, ITakeDamage
{
    public float currentHealth;
    public bool isAlive = true;

    private bool _canTakeDamage = true;
    private EnemyController enemyController;
    private vRagdoll ragdoll;

    [SerializeField] private GameObject[] bloodPoolPrefabs;


    private EnemyAudioController enemyAudio;
    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        ragdoll = GetComponent<vRagdoll>();
        enemyAudio = GetComponent<EnemyAudioController>();
        
        currentHealth = enemyController.enemy.maxHealth;
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
        
        
        //play a random death sound
        enemyAudio.PlayAudioClip(enemyAudio.GetRandomEnemyAudioClip(enemyAudio.deathClips));
        
        StartCoroutine(BloodPoolAfterDeath());

        if (this.name == "Boss")
        {
            GameManager.instance.UnlockFinalLevelThreeExit();
        }
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
