using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class LevelChange : MonoBehaviour
{

   private void OnTriggerEnter(Collider other)
   {
        if (other.CompareTag("Level One"))
        {
            SceneManager.LoadScene("LEVELTWO");
        }

        if (other.CompareTag("Level Two"))
        {
            SceneManager.LoadScene("LEVELTHREE");
        }

        if (other.CompareTag("Level Three"))
        {
            SceneManager.LoadScene("LEVELFOUR");
        }

    }

}
