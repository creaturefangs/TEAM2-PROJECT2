using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }
    public int killCounter = 0;
    public int levelIndex { get; set; }
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
    }

    public void IncrementPlayerKillCount()
    {
        killCounter++;
    }
    
    
}
