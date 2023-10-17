using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickupManager : MonoBehaviour
{
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private float pickupDistance = 2.0f;
    [SerializeField] private KeyCode itemPickupKeyCode;
    
    private PlayerHealth playerHealth;
    private ItemPickupSO itemUnderCursor;
    [SerializeField] private HealthBar healthBar;
    private bool prevHealthStatus = true;  

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void FixedUpdate()
    {
        ItemPickupComponent currentPickupComponent = null;
        bool isLookingAtItem = false;

        if (Physics.Raycast(
                PlayerUI.instance.raycastOrigin.position,
                PlayerUI.instance.raycastOrigin.forward,
                out RaycastHit hitInfo,
                pickupDistance,
                pickupLayerMask))
        {
            var pickup = hitInfo.collider.gameObject.GetComponent<ItemPickupComponent>();

            if (pickup != null)
            {
                currentPickupComponent = pickup;

                if (pickup.CompareTag("Cage"))
                {
                    isLookingAtItem = true;
                    //StartCoroutine(PlayerUI.instance.OpenCageTutorial());
                    if (Input.GetKeyDown(itemPickupKeyCode))
                    {
                        pickup.itemPickupEvent.Invoke();
                        pickup.tag = "Untagged";
                    }
                }

                if (pickup.CompareTag("Syringe"))
                {
                    if (!playerHealth.IsBelowMaxHealth() && prevHealthStatus)
                    {
                        prevHealthStatus = false;
                        StartCoroutine(healthBar.FlashHealthBarGreen(.8f, 3));
                    }
                    else if(playerHealth.IsBelowMaxHealth())
                    {
                        prevHealthStatus = true;
                    }

                    if (playerHealth.IsBelowMaxHealth())
                    {
                        isLookingAtItem = true;
                        pickup.enableDestructionOnPickup = true;

                        if (Input.GetKeyDown(itemPickupKeyCode))
                        {
                            isLookingAtItem = false;

                            pickup.itemPickupEvent.Invoke();

                            if(pickup.enableDestructionOnPickup)
                            {
                                Destroy(pickup.gameObject);
                            }

                            currentPickupComponent = null;
                        }
                    }
                }
                
            }
        }

        PlayerUI.instance.itemPickupText.text = isLookingAtItem && currentPickupComponent != null
            ? $"Press {itemPickupKeyCode} to pickup {currentPickupComponent.itemData.itemName}"
            : "";
    }
    
}