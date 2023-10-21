    using System;
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
            //DoorRaycast();
        }

        private void OnTriggerEnter(Collider col)
        {
            // Check if the collided object is the player
            if(col.gameObject.CompareTag("ExitDoor"))
            {
                // Check if the player hasn't reached 9 kills
                if(GameManager.instance.killCounter < 9)
                {
                    // Display text prompt to the player
                    PlayerUI.instance.doorInteractionText.text = "You must kill all enemies before proceeding!";
            
                    // You may also want to hide this text after some seconds. You can use a Unity Coroutine for this
                    StartCoroutine(PlayerUI.instance.DisableUIElement(PlayerUI.instance.doorInteractionText.gameObject, 5f));
                }
                else
                {
                    // Open the door
                    exitDoor.OpenExitDoor();
                }
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
                            // Check kill count before proceeding
                            if (GameManager.instance.killCounter < 9)
                            {
                                // Show a new text prompt to the player, and hide it after 5 seconds
                                PlayerUI.instance.doorInteractionText.text = "You must kill all enemies before proceeding!";
                                StartCoroutine(PlayerUI.instance.DisableUIElement(PlayerUI.instance.doorInteractionText.gameObject, 5f));
                            }
                            else // If the kill count is 9 or more, proceed to open the door
                            {
                                exitDoor.OpenExitDoor();
                            }
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

            if (!isLookingAtExitDoor || GameManager.instance.killCounter < 9)
            {
                PlayerUI.instance.doorInteractionText.text = "";
            }
            else
            {
                PlayerUI.instance.doorInteractionText.text = "Press [E] to open the exit door.";
            }
        }
    }
