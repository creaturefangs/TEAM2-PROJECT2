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
    private void Awake()
    {
        exitDoor = GameObject.FindObjectOfType<ExitDoor>();
    }
    
    private void Update()
    {
        if (exitDoor.canBeOpened)
        {
            DoorRaycast(); 
        }
        
    }

    private void DoorRaycast()
    {
        bool isLookingAtExitDoor = false;

        if (exitDoor.canBeOpened)
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
