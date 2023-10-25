using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneChange : MonoBehaviour
{
    public float changeTime;
    public string sceneName;
    private void Update()
    {
        changeTime -= Time.deltaTime;
        if (changeTime <= 0)
        {
            SceneManager.LoadScene(sceneName);
            GameManager.instance.ResetPlayerKillCount();

            if (sceneName == "MAINMENU")
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

}



