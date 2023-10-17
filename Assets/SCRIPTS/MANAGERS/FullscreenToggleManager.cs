using UnityEngine;
using UnityEngine.UI;

public class FullscreenToggleManager : MonoBehaviour
{
    private Toggle toggle;
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(ToggleFullscreen);
    }

    public void ToggleFullscreen(bool enabled)
    {
        Screen.fullScreen = enabled;
    }
}