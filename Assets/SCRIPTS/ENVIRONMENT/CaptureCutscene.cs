using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CaptureCutscene : MonoBehaviour
{ 
    public PlayableDirector cutsceneDirector;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cutsceneDirector.Play();
        }
    }

}
