using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupManager : MonoBehaviour
{
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private float pickupDistance = 2.0f;
    [SerializeField] private KeyCode itemPickupKeyCode;

    private ItemPickupSO itemUnderCursor;

    private void LateUpdate()
    {
        ItemPickupSO currentItem = null;
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
                currentItem = pickup.itemData;
                isLookingAtItem = true;

                if (Input.GetKeyDown(itemPickupKeyCode))
                {
                    isLookingAtItem = false;
                    
                    //Invoke an event depending on the item that is picked up
                    pickup.itemPickupEvent.Invoke();
                    currentItem = null;
                }
            }
            else
            {
                isLookingAtItem = false;
            }
        }
        
        // Update the text only when the state changes (if looking at an item, show the pickup text, else pickup text is empty
        PlayerUI.instance.itemPickupText.text = isLookingAtItem
            ? $"Press {itemPickupKeyCode} to pickup {currentItem.itemName}"
            : "";
        
        itemUnderCursor = currentItem;
    }
}