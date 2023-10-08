using System.Collections;
using UnityEngine;
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
        switch (GameManager.instance.killCounter)
        {
            case 5: 
                _playerHealth.IncreaseAdditionalHealth(50);
                PlayerUI.instance.ShowAdditionalHealth(_playerHealth.additionalHealth);
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
        _meleeController.DamageBuff = 50.0f;
        yield return new WaitForSeconds(rageDuration);
        _meleeController.DamageBuff = 0;
    }
}
