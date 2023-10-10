using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class KillstreakManager : MonoBehaviour
{
    private PlayerHealth _playerHealth;
    private PlayerMeleeController _meleeController;
    private Freeze _freeze;
    [Header("Killstreak Cooldowns")]
    [SerializeField] private float rageDuration = 20.0f;
    private void Awake()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _meleeController = GetComponent<PlayerMeleeController>();
        _freeze = GetComponent<Freeze>();
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
        switch (GameManager.instance.killCounter)
        {
            //Increased health
            case 5: 
                _playerHealth.IncreaseAdditionalHealth(50);
                PlayerUI.instance.ShowAdditionalHealth(_playerHealth.additionalHealth);
                PlayerUI.instance.ShowNextKillstreakImage(PlayerUI.instance.rageAbilityImage);
                break;
            //Rage ability - Increased damage
            case 10:
                PlayerUI.instance.EnableUIElement(SceneManager.GetActiveScene().name == "LEVELONE"
                    ? PlayerUI.instance.fireRageParticles
                    : PlayerUI.instance.iceRageParticles);
                StartCoroutine(StartTemporaryDamageBuff());
                PlayerUI.instance.ShowNextKillstreakImage(PlayerUI.instance.freezeAbilityImage);
                break;
            //Freeze ability - freeze enemies in area
            case 15:
                _freeze.canFreeze = true;
                PlayerUI.instance.ShowNextKillstreakImage(null);
                break;
        }
    }

    private IEnumerator StartTemporaryDamageBuff()
    {
        _meleeController.DamageBuff = 50.0f;
        yield return new WaitForSeconds(rageDuration);
        _meleeController.DamageBuff = 0;
    }
}
