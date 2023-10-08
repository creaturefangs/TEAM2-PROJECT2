using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }
    public int killCounter = 0;
    public int killsToNextKillStreak = 5;
    public int maxKillsToNextKillStreak = 5;
    public int levelIndex { get; set; }
    
    public Vector3 lastCheckpointPosition { get; set; }
    
    private void Awake() 
    {
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);
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
    }

    public void IncrementPlayerKillCount()
    {
        killCounter++;
        killsToNextKillStreak--;
        if (killsToNextKillStreak <= 0)
        {
            killsToNextKillStreak = 5;
        }
        
        PlayerUI.instance.UpdateKillsUI(killCounter, killsToNextKillStreak);
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
}
