using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public GameObject characterpanelUI;
    public GameObject playerUI;
    public GameObject pauseMenuUI;
    public bool CPVisible = false;
    public PauseManager pauseManager;
    public AudioSource CPSFX;

    void Start()
    {
        pauseManager = GetComponent<PauseManager>();

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (pauseManager.GameIsPaused == true)
            {
                CPVisible = false;

            }
            else
            {
                if (CPVisible)
                {
                    Invisible();
                }
                else
                {
                    Visible();
                }
            }

        }
    }

    public void Invisible()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        characterpanelUI.SetActive(false);
        playerUI.SetActive(true);
        CPVisible = false;
        CPSFX.Play();
    }

    public void Visible()
    {

        characterpanelUI.SetActive(true);
        playerUI.SetActive(false);
        CPVisible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CPSFX.Play();
    }

}
