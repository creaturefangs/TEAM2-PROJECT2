using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHealth : MonoBehaviour, ITakeDamage
{
    [SerializeField] private HealthBar healthBar;
    [FormerlySerializedAs("_health")] public float health;
    public float maxHealth = 100;
    public bool isAlive = true;
    private void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health);
    }

    public void ApplyDamage(float amount)
    {
        health -= amount;
    }

    public void TakeDamage(float amount)
    {
        ApplyDamage(amount);
        healthBar.UpdateHealthBar(health);
        if (health <= 0)
        {
            Die();
        }
    }

    public void IncreaseHealth(float amount)
    {
        health += amount;
        healthBar.UpdateHealthBar(health);
    }

    public void Die()
    {
        isAlive = false;
    }
}
