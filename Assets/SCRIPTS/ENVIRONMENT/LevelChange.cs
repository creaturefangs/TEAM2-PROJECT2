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
            GameManager.instance.LoadLevel("LEVELTWO"); // Load the next level
            GameManager.instance.lastCheckpointPosition = new Vector3(770.3599f, -67.594f, 433.8945f)/* Level Two spawn position */;
        }

        if (other.CompareTag("Level Two"))
        {
            GameManager.instance.ResetPlayerKillCount();
            GameManager.instance.recentLevelIndex++; // increment level index
            
            GameManager.instance.LoadLevel("LEVELTHREE"); // Load the next level
            
            GameManager.instance.lastCheckpointPosition = new Vector3(-345.59f, -539.78f, 493.1f); /* Level Three spawn position */
        }

        if (other.CompareTag("Level Three"))
        {
            GameManager.instance.ResetPlayerKillCount();
            GameManager.instance.recentLevelIndex++; // increment level index
            
            GameManager.instance.LoadLevel("LEVELFOUR"); // Load the next level
            
            GameManager.instance.lastCheckpointPosition = new Vector3(817.5f, 11.31f, 493.1f); /* Level four spawn position */
        }
    }
}