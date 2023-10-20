using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Level One"))
        {
            GameManager.instance.ResetPlayerKillCount();
            GameManager.instance.recentLevelIndex++; // increment level index
            GameManager.instance.recentSceneName = "LEVELTWO"; // saving level name
            
            SceneManager.LoadScene("LEVELTWO");
            
            GameManager.instance.lastCheckpointPosition = transform.position; //reset player position when changing levels
        }

        if (other.CompareTag("Level Two"))
        {
            GameManager.instance.ResetPlayerKillCount();
            GameManager.instance.recentLevelIndex++; // increment level index
            GameManager.instance.recentSceneName = "LEVELTHREE"; // saving level name
            
            SceneManager.LoadScene("LEVELTHREE");
            
            GameManager.instance.lastCheckpointPosition = transform.position; //reset player position when changing levels
        }

        if (other.CompareTag("Level Three"))
        {
            GameManager.instance.ResetPlayerKillCount();
            GameManager.instance.recentLevelIndex++; // increment level index
            GameManager.instance.recentSceneName = "LEVELFOUR"; // saving level name
            
            SceneManager.LoadScene("LEVELFOUR");
            
            GameManager.instance.lastCheckpointPosition = transform.position; //reset player position when changing levels
        }
    }
}