using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }
    public int killCounter = 0;
    public int killsToNextKillStreak = 4;
    public int maxKillsToNextKillStreak = 4;
    
    
    public string recentSceneName { get; set; }
    public int levelIndex { get; set; }
    public int recentLevelIndex { get; set; }
    public Vector3 lastCheckpointPosition { get; set; }
    
    private bool _isRespawning = false;
    
    public static event Action OnPlayerReached9Kills;
    
    private void Awake() 
    {
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    /// <summary>
    /// When player completes a level, call GameManager.instance.IncrementLevelCount(), then call GameManager.instance.LoadLevel(GameManager.instance.levelIndex)
    /// </summary>
    /// <param name="currentLevel"></param>
    public void LoadLevel(int currentLevel)
    {
        switch (levelIndex)
        {
            case 0: SceneManager.LoadScene("LEVELONE");
                break;
            case 1: SceneManager.LoadScene("LEVELTWO");
                            break;
            case 2: SceneManager.LoadScene("LEVELTHREE");
                            break;
            case 3: SceneManager.LoadScene("LEVELFOUR");
                break;
            default: SceneManager.LoadScene("MAINMENU");
                break;
        }
        ResetPlayerKillCount();
        _isRespawning = true;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // check if the player was respawning
        if (_isRespawning)
        {
            // reset respawning
            _isRespawning = false;

            // ensure that player character exists
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            int attempts = 0;
            //if player isnt found, try up to 100 attempts to find the player.
            while(player == null && attempts < 100)
            {
                player = GameObject.FindGameObjectWithTag("Player");
                attempts++;
            }

            if(player != null)
            {
                player.transform.position = lastCheckpointPosition;
            }
            else
            {
                Debug.LogWarning("Player object not found - possibly because the current scene (" + scene.name + ") does not contain a player object.");
            }
        }
    }

    public void IncrementPlayerKillCount()
    {
        killCounter++;
        killsToNextKillStreak--;
        Debug.Log("Kill count: " + killCounter);

        if (killsToNextKillStreak <= 0)
        {
            PlayerUI.instance.UpdateKillsUI(killCounter, maxKillsToNextKillStreak);
            GameObject.FindGameObjectWithTag("Player").GetComponent<KillstreakManager>().GrantKillStreaks();
            killsToNextKillStreak = maxKillsToNextKillStreak;
        }
        else
        {
            PlayerUI.instance.UpdateKillsUI(killCounter, killsToNextKillStreak);
        }

        // Check if the player has reached 9 kills.
        if (killCounter >= 9)
        {
            // Raise the event to notify the exit door.
            OnPlayerReached9Kills?.Invoke();
        }
    }

    
    
    public void Die()
    {
        recentLevelIndex = levelIndex;
        SceneManager.LoadScene("GAMEOVER");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void ResetPlayerKillCount()
    {
        killCounter = 0;
        PlayerUI.instance.UpdateKillsUI(killCounter, maxKillsToNextKillStreak);
    }

    /// <summary>
    /// When the player dies and is brought to the death scene, call this method to bring the player back to their last checkpoint.
    /// </summary>
    /// <param name="playerTransform"></param>
    public void LoadCheckpointOnDeath(Transform playerTransform)
    {
        playerTransform.transform.position = lastCheckpointPosition;
    }

    public void UnlockFinalLevelThreeExit()
    {
        LevelThreeExitTrigger exit = GameObject.FindGameObjectWithTag("Level Three").GetComponent<LevelThreeExitTrigger>();
        if (exit != null)
        {
            exit.OnBossDeathEnableCollider();
        }
    }
}
