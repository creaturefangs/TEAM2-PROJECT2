using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CaptureCutscene : MonoBehaviour
{ 
    public PlayableDirector cutsceneDirector;
    public GameObject player;
    public GameObject camera;
    public GameObject cutsceneCamera;

    
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
            camera.SetActive(false);
            cutsceneCamera.SetActive(true);
        }
    }

}
