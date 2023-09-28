using UnityEngine;

[CreateAssetMenu (fileName = "Item Pickups", menuName = "Item Pickup/Pickup", order = 4)]
public class ItemPickupSO : ScriptableObject
{
    public string itemTag; // You could remove this, if you don't need it for another purpose 
    public string itemName;
    public GameObject itemPrefab; 
}
