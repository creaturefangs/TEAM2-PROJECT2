using UnityEngine;

[CreateAssetMenu (fileName = "Item Pickups", menuName = "Item Pickup/Pickup", order = 4)]
public class ItemPickupSO : ScriptableObject
{
    public string itemTag; // pretty much only used to compare tags when raycasting
    public string itemName; // name of the item, used for showing pickup message
    public GameObject itemPrefab; // only needed if item is instantiated
}
