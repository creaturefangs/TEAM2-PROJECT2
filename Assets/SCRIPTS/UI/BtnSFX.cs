using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnSFX : MonoBehaviour
{

    public AudioSource btnSFX;
    public AudioClip hoverSFX;
    public AudioClip clickSFX;

   public void HoverSound()
    {
        btnSFX.PlayOneShot(hoverSFX);
    }    

    public void ClickSound()
    {
        btnSFX.PlayOneShot(clickSFX);
    }
}
