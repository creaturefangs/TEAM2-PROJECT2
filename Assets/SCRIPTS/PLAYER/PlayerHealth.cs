using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerHealth : MonoBehaviour, ITakeDamage, IInvincibility
{
    [Header("Main Health")]
    [SerializeField] private HealthBar healthBar;
    public float health;
    public float maxHealth = 100;
    public float additionalHealth = 0; // if player has earned additional health
    public bool isAlive = true; // player is alive?

    [Header("Invincibility")] 
    [SerializeField] private float invincibilityTimeout = 5.0f; //how long player is invincible for
    
    [Tooltip("Mainly for easier testing! Enable to take no damage from enemies :)")]
    public bool _isInvincible = false;

    private PlayerMeleeController melee;
    private void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health);

        melee = GetComponent<PlayerMeleeController>();
    }

    public bool CanTakeHit { get; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Syringe"))
        {
            if (IsBelowMaxHealth())
            {
                ItemPickupComponent pickup = other.GetComponent<ItemPickupComponent>();
                pickup.itemPickupEvent.Invoke();
                pickup.enableDestructionOnPickup = true;
                Destroy(other.gameObject);
            }
        }
        
        if (other.CompareTag("Flamethrower"))
        {
            TakeDamage(20);
        }
    }

    public void ApplyDamage(float amount)
    {
        if(additionalHealth > 0)
        {
            if(additionalHealth >= amount)
            {
                additionalHealth -= amount;
                amount = 0;
            }
            else 
            {
                amount -= additionalHealth;
                additionalHealth = 0;
            }
            
            PlayerUI.instance.ShowAdditionalHealth(additionalHealth);
        }

        health -=  amount;
        healthBar.UpdateHealthBar(health);
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
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        healthBar.UpdateHealthBar(health);
    }

    public void IncreaseAdditionalHealth(float amount)
    {
        additionalHealth += amount;
        // Ensure health + additional health doesnt exceed max health
        if(health + additionalHealth > maxHealth)
        {
            additionalHealth = maxHealth - health;
        }
        health += additionalHealth;
        PlayerUI.instance.ShowAdditionalHealth(additionalHealth);
        healthBar.UpdateHealthBar(health + additionalHealth);
    }

    public bool IsBelowMaxHealth() => health + additionalHealth < maxHealth;
    
    public void Die()
    {
        if(additionalHealth > 0) 
            return;
      
        isAlive = false;
        
        health = maxHealth;
        additionalHealth = 0;
        PlayerUI.instance.ShowAdditionalHealth(additionalHealth);

        GameManager.instance.Die();
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
    
    public void EnableMaxDamage()
    {
        throw new NotImplementedException();
    }
}
