using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class CaptureCutscene : MonoBehaviour
{ 
    public PlayableDirector cutsceneDirector;
    public GameObject player;
    [FormerlySerializedAs("camera")] public GameObject playerCamera; //renamed just in case of confusion :D
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
            playerCamera.SetActive(false);
            cutsceneCamera.SetActive(true);
        }
    }

}
