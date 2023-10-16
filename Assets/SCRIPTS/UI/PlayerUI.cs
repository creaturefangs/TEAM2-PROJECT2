using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI instance; //persist Player UI instance through game, to access, call PlayerUI.instance.Method/Variable
    [HideInInspector] public PauseManager pauseManager;
    public Transform raycastOrigin;
    public Transform feetPosition;
    public GameObject rageAbilityParticles;
    
    
    [Header("UI Elements related to powerups/abilities")]
    public GameObject freezeImage;
    
    
    [Header("Player UI Elements")]
    public TMP_Text itemPickupText;
    public TMP_Text additionalHealthText;
    public TMP_Text killsText; //show current kills
    public TMP_Text killsToNextKillStreakText; //show remaining kills to unlock next killstreak
    public TMP_Text checkpointAlertText; //show that player has reached a checkpoint briefly
    public TMP_Text killStreakTutorialText; //briefly show what the player earns with each killstreak
    public TMP_Text doorInteractionText; //Only to be used in level three
    public TMP_Text cageText;
    public Image nextKillStreakImage; // Will show an image of the next killstreak
    public Sprite additionalHealthImage;
    public Sprite rageAbilityImage;
    public Sprite freezeAbilityImage;
    public GameObject killStreakUI;
    public GameObject fireRageUIParticles;
    public GameObject fireRageUIBorder;
    public GameObject iceRageParticles;
    
    private void Awake()
    {
        #region PlayerUI singleton
        //persist Player UI instance through game, to access, call PlayerUI.instance.Method/Variable
        if (instance == null && instance != this)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        #endregion
    }

    private void Start()
    {
        pauseManager = GetComponent<PauseManager>();
    }

    /// <summary>
    /// Will show a ui element, should be a publicly declared GameObject in PlayerUI
    /// </summary>
    /// <param name="element"></param>
    public void EnableUIElement(GameObject element)
    {
        element.SetActive(!element.activeSelf);
    }

    /// <summary>
    /// Disable a ui element after a period of time, should be a publicly declared GameObject in PlayerUI
    /// </summary>
    /// <param name="element"></param>
    /// <param name="disableTimer"></param>
    /// <returns></returns>
    public IEnumerator DisableUIElement(GameObject element, float disableTimer)
    {
        yield return new WaitForSeconds(disableTimer);
        element.SetActive(false);
    }

    public void ShowAdditionalHealth(float amount)
    {
        additionalHealthText.text = " + " + amount.ToString("0");
    }

    public void UpdateKillsUI(int kills, int killsToNextKillStreak)
    {
        killsText.text = "Kills: " + kills;
        killsToNextKillStreakText.text = "Kills remaining to next killstreak: " + killsToNextKillStreak;
    }

    public void ShowCheckpointUnlock(int index)
    {
        checkpointAlertText.text = "Checkpoint " + index + " reached!";
    }

    public void ShowNextKillStreakImage(Sprite image)
    {
        nextKillStreakImage.sprite = image;
    }

    public IEnumerator OpenCageTutorial()
    {
        cageText.text = "Press [F] to escape the cage";
        yield return new WaitForSeconds(4.0f);
        cageText.text = "";
    }
}
