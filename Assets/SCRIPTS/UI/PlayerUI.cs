using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI instance;
    
    [Header("UI Elements related to powerups/abilities")]
    public GameObject freezeImage;
    private void Awake()
    {
        #region PlayerUI singleton
        //persist Player UI instance through game, to access, call PlayerUI.instance.Method/Variable
        if (instance == null && instance != this)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        #endregion
    }

    public void EnableUIElement(GameObject element)
    {
        element.SetActive(!element.activeSelf);
    }
}
