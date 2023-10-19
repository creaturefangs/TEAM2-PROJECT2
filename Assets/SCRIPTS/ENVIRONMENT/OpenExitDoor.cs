using UnityEngine;
using UnityEngine.InputSystem;

public class OpenExitDoor : MonoBehaviour
{
    /// <summary>
    /// This script should only be on the Player in level three!
    /// </summary>
    [SerializeField] private LayerMask doorLayer;

    private const float InteractionDistance = 3.5f;
    private ExitDoor exitDoor;

    private bool canOpenDoor = false; // Track whether the door can be opened

    private void Awake()
    {
        exitDoor = GameObject.FindObjectOfType<ExitDoor>();

        // Subscribe to the event when this script is enabled.
        GameManager.OnPlayerReached9Kills += HandlePlayerReached9Kills;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event when this script is destroyed to prevent memory leaks.
        GameManager.OnPlayerReached9Kills -= HandlePlayerReached9Kills;
    }

    private void HandlePlayerReached9Kills()
    {
        // The player has reached 9 kills, so the door can be opened.
        canOpenDoor = true;
    }
    
    private void Update()
    {
        if (canOpenDoor)
        {
            DoorRaycast(); 
        }
    }

    private void DoorRaycast()
    {
        bool isLookingAtExitDoor = false;

        if (canOpenDoor)
        {
            if (Physics.Raycast(
                    PlayerUI.instance.raycastOrigin.position,
                    PlayerUI.instance.raycastOrigin.forward,
                    out RaycastHit hitInfo,
                    InteractionDistance,
                    doorLayer))
            {
                if (hitInfo.collider.CompareTag("ExitDoor"))
                {
                    isLookingAtExitDoor = true;
                    if (Keyboard.current.eKey.wasPressedThisFrame)
                    {
                        exitDoor.OpenExitDoor();
                    }
                }
                else
                {
                    isLookingAtExitDoor = false;
                }
            }
            else
            {
                isLookingAtExitDoor = false;
            }
        }
        
        PlayerUI.instance.doorInteractionText.text = isLookingAtExitDoor ? "Press [E] to open the exit door." : "";
    }
}
