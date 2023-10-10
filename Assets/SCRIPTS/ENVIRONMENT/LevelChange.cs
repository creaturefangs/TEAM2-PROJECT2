using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
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