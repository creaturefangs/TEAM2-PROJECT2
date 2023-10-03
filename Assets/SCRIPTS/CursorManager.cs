using UnityEngine;

public class CursorManager : MonoBehaviour
{
    //Attach this script to the game manager!
    public void ShowAndEnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideAndDisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
