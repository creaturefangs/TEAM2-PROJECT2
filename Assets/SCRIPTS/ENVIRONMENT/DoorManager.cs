using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour

{
    private Animator doorAnimator;
    private bool isOpen = false;

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
        }
    }
}


