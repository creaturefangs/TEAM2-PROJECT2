using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, ITakeDamage
{
    [SerializeField] private HealthBar healthBar;
    private float _health;
    private float maxHealth = 100;
    public bool isAlive = true;
    private void Start()
    {
        _health = maxHealth;
        healthBar.UpdateHealthBar(_health);
    }

    public void ApplyDamage(float amount)
    {
        _health -= amount;
    }

    public void TakeDamage(float amount)
    {
        ApplyDamage(amount);
        healthBar.UpdateHealthBar(_health);
        if (_health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isAlive = false;
    }
}
