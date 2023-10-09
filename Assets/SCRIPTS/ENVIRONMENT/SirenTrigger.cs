using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirenTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource sirenAudioSource;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sirenAudioSource.Play();
            Destroy(gameObject);
        }
    }
}
