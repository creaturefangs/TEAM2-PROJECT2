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

    private void Start()
    {
        PlayerUI.instance.UpdateKillsUI(GameManager.instance.killCounter,
            GameManager.instance.maxKillsToNextKillStreak);
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
            case 4: 
                _playerHealth.IncreaseAdditionalHealth(50);
                PlayerUI.instance.ShowAdditionalHealth(_playerHealth.additionalHealth);
                PlayerUI.instance.ShowNextKillStreakImage(PlayerUI.instance.rageAbilityImage);
                break;
            //Rage ability - Increased damage
            case 8:
                // PlayerUI.instance.EnableUIElement(SceneManager.GetActiveScene().name == "LEVELONE"
                //     ? PlayerUI.instance.fireRageParticles
                //     : PlayerUI.instance.iceRageParticles);
                // increase damage temporarily
                SpawnRageParticle();
                PlayerUI.instance.EnableUIElement(PlayerUI.instance.fireRageUIParticles);
                PlayerUI.instance.EnableUIElement(PlayerUI.instance.fireRageUIBorder);
                StartCoroutine(PlayerUI.instance.DisableUIElement(PlayerUI.instance.fireRageUIParticles, rageDuration));
                StartCoroutine(PlayerUI.instance.DisableUIElement(PlayerUI.instance.fireRageUIBorder, rageDuration));
                StartCoroutine(StartDamageBuff());
                //increase damage permanently
                //StartDamageBuff();
                _playerHealth.IncreaseAdditionalHealth(50);
                PlayerUI.instance.ShowAdditionalHealth(_playerHealth.additionalHealth);
                PlayerUI.instance.ShowNextKillStreakImage(PlayerUI.instance.freezeAbilityImage);
                break;
            //Freeze ability - freeze enemies in area
            case 12:
                _freeze.canFreeze = true;
                PlayerUI.instance.nextKillStreakImage.gameObject.SetActive(false);
                break;
        }
    }

    // private void StartDamageBuff()
    // {
    //     _meleeController.DamageBuff = 50.0f;
    // }

    private IEnumerator StartDamageBuff()
    {
        _meleeController.DamageBuff = 50.0f;
        yield return new WaitForSeconds(rageDuration);
        _meleeController.DamageBuff = 0.0f;
    }

    private void SpawnRageParticle()
    {
        GameObject fire = Instantiate(PlayerUI.instance.rageAbilityParticles, PlayerUI.instance.feetPosition.position,
            Quaternion.identity);

        fire.transform.localScale = new Vector3(.05f, .05f, .05f);
        fire.transform.SetParent(PlayerUI.instance.feetPosition);

        Destroy(fire, rageDuration);
    }
}
