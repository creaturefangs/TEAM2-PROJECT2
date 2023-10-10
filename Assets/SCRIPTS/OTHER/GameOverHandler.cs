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
        GameManager.instance.LoadLevel(GameManager.instance.recentLevelIndex);
    }
}
