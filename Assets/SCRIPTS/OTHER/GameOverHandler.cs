using UnityEngine;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{
    [SerializeField] private Button tryAgainButton;

    private void Awake()
    {
        tryAgainButton.onClick.AddListener(HandleTryAgainClicked);
    }

    private void HandleTryAgainClicked()
    {
        string recentSceneName = GameManager.instance.recentSceneName;
        if (!string.IsNullOrEmpty(recentSceneName))
        {
            GameManager.instance.LoadLevel(recentSceneName);
        }
        else
        {
            // Handle the case when recentSceneName is empty or not set.
            // You can load a default level or return to the main menu.
            // Example:
            // GameManager.instance.LoadLevel("MAINMENU");
        }
    }
}