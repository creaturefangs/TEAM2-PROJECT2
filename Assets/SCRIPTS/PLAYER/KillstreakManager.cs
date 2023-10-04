using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class KillstreakManager : MonoBehaviour
{
    private PlayerHealth _playerHealth;
    private PlayerMeleeController _meleeController;

    [Header("Killstreak Cooldowns")]
    [SerializeField] private float rageDuration = 20.0f;
    private void Awake()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _meleeController = GetComponent<PlayerMeleeController>();
    }

    private void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame && !PlayerUI.instance.pauseManager.gameIsPaused)
        {
            PlayerUI.instance.killStreakUI.SetActive(!PlayerUI.instance.killStreakUI.activeSelf);
        }
    }

    public void GrantKillStreaks()
    {
        //TODO: Update player texture overlay the more kills they get(blood builds up over time on the player)
        switch (GameManager.instance.killCounter)
        {
            case 5: 
                _playerHealth.IncreaseHealth(50);
                PlayerUI.instance.ShowAdditionalHealth(_playerHealth.health);
                break;
            case 10:
                PlayerUI.instance.EnableUIElement(SceneManager.GetActiveScene().name == "LEVELONE"
                    ? PlayerUI.instance.fireRageParticles
                    : PlayerUI.instance.iceRageParticles);
                StartCoroutine(StartTemporaryDamageBuff()); 
                break;
        }
    }

    private IEnumerator StartTemporaryDamageBuff()
    {
        var attackDamage = _meleeController.AttackDamage();
        attackDamage += 50.0f;
        Debug.Log("attackDamage: " + attackDamage);
        
        yield return new WaitForSeconds(rageDuration);
        
        attackDamage -= 50.0f;
    }
}
