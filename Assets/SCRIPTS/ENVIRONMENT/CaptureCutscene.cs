using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class CaptureCutscene : MonoBehaviour
{ 
    public PlayableDirector cutsceneDirector;
    public GameObject player;
    public GameObject playerCamera; //renamed just in case of confusion :D
    public GameObject cutsceneCamera;
    public float changeTime;
    public string sceneName;


    void Start()
    {
        GameObject camera = GameObject.Find("ThirdPersonCamera");
        cutsceneCamera.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cutsceneDirector.Play();
            player.SetActive(false);
            playerCamera.SetActive(false);
            cutsceneCamera.SetActive(true);

            changeTime -= Time.deltaTime;
            if (changeTime <= 0)
            {
                SceneManager.LoadScene(sceneName);
            }

        }
    }
}
