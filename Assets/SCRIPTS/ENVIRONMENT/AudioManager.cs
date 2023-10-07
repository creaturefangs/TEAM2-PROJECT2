using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour

{
    public AudioSource audioManager;
    public AudioClip[] Music;
  

    // Start is called before the first frame update
    void Start()
    {
        PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic()
    {

         // checks if player is colliding with a music box cllider, and if so, play a certain abient soundtrack. 
         // if the player is not colliding with a certain box collider, the base music plays
    }
}
