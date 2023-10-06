using Unity.VisualScripting;
using UnityEngine;

public class ItemPickupManager : MonoBehaviour
{
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private float pickupDistance = 2.0f;
    [SerializeField] private KeyCode itemPickupKeyCode;
    
    private PlayerHealth playerHealth;
    private ItemPickupSO itemUnderCursor;
    [SerializeField] private HealthBar healthBar;
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
                isLookingAtItem = true;

                
                if (pickup.CompareTag("Syringe"))
                {
                    if (!playerHealth.IsBelowMaxHealth()) StartCoroutine(healthBar.FlashHealthBarGreen(.75f));
                    pickup.enableDestructionOnPickup = true;
                }
                
                if (Input.GetKeyDown(itemPickupKeyCode))
                {
                    isLookingAtItem = false;
                
                    //Invoke an event depending on the item that is picked up
                    pickup.itemPickupEvent.Invoke();

                    //If destruction is enabled on pickup, destroy the item gameObject
                    if(pickup.enableDestructionOnPickup)
                    {
                        Destroy(pickup.gameObject);
                    }

                    
                
                    currentPickupComponent = null;
                }
            }
            else 
            {
                isLookingAtItem = false;
            }
        }
    
        // Update the text only when the state changes (if looking at an item, show the pickup text, else pickup text is empty
        PlayerUI.instance.itemPickupText.text = isLookingAtItem
            ? $"Press {itemPickupKeyCode} to pickup {currentPickupComponent.itemData.itemName}"
            : "";
    }
}