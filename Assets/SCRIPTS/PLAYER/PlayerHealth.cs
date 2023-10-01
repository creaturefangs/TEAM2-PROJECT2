using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHealth : MonoBehaviour, ITakeDamage, IInvincibility
{
    [Header("Main Health")]
    [SerializeField] private HealthBar healthBar;
    public float health;
    public float maxHealth = 100;
    public bool isAlive = true; // player is alive?

    [Header("Invincibility")] 
    [SerializeField] private float invincibilityTimeout = 5.0f; //how long player is invincible for
    private bool _isInvincible = false;
    private void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health);
    }

    public void ApplyDamage(float amount) //never directly call this method. use take damage to take damage.
    {
        health -= amount;
    }

    public void TakeDamage(float amount) // never to be directly called. use ITakeDamage component to TakeDamage
    {
        if (!_isInvincible)
        {
            ApplyDamage(amount);
            healthBar.UpdateHealthBar(health);
            if (health <= 0)
            {
                Die();
            }
        }
    }

    public void IncreaseHealth(float amount)
    {
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
        
        health += amount;
        healthBar.UpdateHealthBar(health);
    }

    public bool IsBelowMaxHealth()
    {
        return health < maxHealth;
    }
    
    public void Die()
    {
        isAlive = false;
    }

    public IEnumerator StartInvincibility()
    {
        _isInvincible = true;
        yield return new WaitForSeconds(invincibilityTimeout);
        DisableInvincibility();
    }

    public void DisableInvincibility()
    {
        _isInvincible = false;
    }
}
