using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI instance; //persist Player UI instance through game, to access, call PlayerUI.instance.Method/Variable
    [HideInInspector] public PauseManager pauseManager;
    public Transform raycastOrigin;
    [Header("UI Elements related to powerups/abilities")]
    public GameObject freezeImage;
    
    [Header("Player UI Elements")]
    public TMP_Text itemPickupText;
    public TMP_Text additionalHealthText;
    public TMP_Text killsText; //show current kills
    public TMP_Text killsToNextKillStreakText; //show remaining kills to unlock next killstreak
    public TMP_Text checkpointAlertText; //show that player has reached a checkpoint briefly
    public Image nextKillStreakImage; // WIll show an image of the next killstreak
    public Sprite additionalHealthImage;
    public Sprite rageAbilityImage;
    public GameObject killStreakUI;
    public GameObject fireRageParticles;
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

    public void ShowAdditionalHealth(float amount)
    {
        additionalHealthText.text = " + " + amount.ToString("0");
    }

    public void UpdateKillsUI(int kills, int killsToNextKillStreak)
    {
        killsText.text = "Kills: " + kills;
        killsToNextKillStreakText.text = "Kills remaining to next killstreak: " + killsToNextKillStreak;
    }

    public IEnumerator ShowCheckpointUnlock(int index)
    {
        checkpointAlertText.text = "Checkpoint " + index + " reached!";
        yield return new WaitForSeconds(2.5f);
        checkpointAlertText.text = "";
    }

    public void ShowNextKillstreakImage(Sprite image)
    {
        nextKillStreakImage.sprite = image;
    }
}
