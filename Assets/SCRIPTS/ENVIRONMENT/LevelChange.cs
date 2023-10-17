using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Level One") || other.CompareTag("Level Two") || other.CompareTag("Level Three"))
        {
            //Reset kills when changing levels
            //Was causing bugs with killstreaks. Commented out now as it shouldn't be needed.
            //GameManager.instance.ResetPlayerKillCount();
        }
        
        if (other.CompareTag("Level One"))
        {
            GameManager.instance.recentLevelIndex++; // increment level index
            SceneManager.LoadScene("LEVELTWO");
        }

        if (other.CompareTag("Level Two"))
        {
            GameManager.instance.recentLevelIndex++; // increment level index
            SceneManager.LoadScene("LEVELTHREE");
        }

        if (other.CompareTag("Level Three"))
        {
            GameManager.instance.recentLevelIndex++; // increment level index
            SceneManager.LoadScene("LEVELFOUR");
        }
    }
}