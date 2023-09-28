using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, ITakeDamage
{
    private float _health;
    private float maxHealth = 100;

    private void Start()
    {
        _health = maxHealth;
        PlayerUI.instance.UpdateHealth(_health);
    }

    public void ApplyDamage(float amount)
    {
        _health -= amount;
    }

    public void TakeDamage(float amount)
    {
        ApplyDamage(amount);
        PlayerUI.instance.UpdateHealth(_health);
        if (_health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }
}
