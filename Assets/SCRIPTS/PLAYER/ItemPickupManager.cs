using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupManager : MonoBehaviour
{
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private float pickupDistance = 2.0f;
    [SerializeField] private KeyCode itemPickupKeyCode;
    
    private ItemPickupSO itemUnderCursor;

    private void Update()
    {
        ItemPickupSO currentItem = null;
        
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
                if (Input.GetKeyDown(itemPickupKeyCode))
                {
                    Destroy(hitInfo.collider.gameObject);
                    //Invoke an event depending on the item that is picked up
                }
            }
        }

        if(currentItem != itemUnderCursor)
        {
            itemUnderCursor = currentItem;

            // Update the text only if the item under cursor has changed
            PlayerUI.instance.itemPickupText.text = itemUnderCursor != null ? $"Press {itemPickupKeyCode} to pickup {itemUnderCursor.itemName}" : "";
        }
    }
}
