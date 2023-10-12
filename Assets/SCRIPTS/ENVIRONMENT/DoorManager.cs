using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class DoorManager : MonoBehaviour

{
    private Animator doorAnimator;
    private bool isOpen = false;
    public TMP_Text doorTxt;
    //enemiesAlive = true;

    private void Start()
    {
        doorAnimator = GetComponent<Animator>();
        isOpen = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            // Play the "DoorOpen" animation.
            doorAnimator.SetBool("Open", true);
            isOpen = true;

            //have the door open after a certain number of enemies are dead
        }

        if ( other.CompareTag("Player") && !isOpen && )
        {
            doorTxt.text = "The door won't budge.";
            isOpen = false;
        }
    }
}


