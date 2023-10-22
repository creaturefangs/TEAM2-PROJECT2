using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class CaptureCutscene : MonoBehaviour
{ 
    public PlayableDirector cutsceneDirector; // timeline object
    public GameObject player; // player object
    //public GameObject playerCamera; //renamed just in case of confusion :D
    public GameObject cutscene; // cutscene timeline
    public GameObject playerUI; //playerui object
    public GameObject thirdpersoncamera;
    int raycastDistance = 10;

    void Start()
    {
        cutscene.SetActive(false);
        player.SetActive(true);
        
    }

    void Update()
    {
        GameObject thirdpersoncamera = GameObject.Find("ThirdPersonCamera");

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance)) //checking if collision between player and cutscene trigger is there
        {
            if (hit.collider.CompareTag("Player"))
            {
                //thirdpersoncamera.SetActive(false); // third person camera is deactivated
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  //checking collision for cutscene trigger
        {
            cutsceneDirector.Play(); // plays the cutscene timeline
            //player.SetActive(false);
           //playerCamera.SetActive(false);
            cutscene.SetActive(true);
            thirdpersoncamera.SetActive(false);
            playerUI.SetActive(false);
        }
    }
}
