using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void OnQuitButtonClick()
    {
        Application.Quit();
    }

    public void OnHelpButtonClick()
    {
        SceneManager.LoadScene("HELPMENU");
    }

    public void OnLevelOneClick()
    {
        SceneManager.LoadScene("LEVELONE");
    }

    public void OnCreditButtonClick()
    {
        SceneManager.LoadScene("CREDITS");
    }

    public void OnMainMenuButtonClick()
    {
        SceneManager.LoadScene("MAINMENU");
    }

    public void OnClickPlaytest()
    {
        SceneManager.LoadScene("TESTLEVEL");
    }

    public void OnClickCutsceneOne()
    {
        SceneManager.LoadScene("INTROCUTSCENE");
    }

    public void OnClickLoadScreen()
    {
        SceneManager.LoadScene("LOADSCENE");
    }

    public void OnClickSettings()
    {
        SceneManager.LoadScene("SETTINGSSCENE");
    }
    
    public void OpenURL(string URLname)
    {
        Application.OpenURL(URLname);
    }
}
