using UnityEngine;
using UnityEngine.Events;

public class ItemPickupComponent : MonoBehaviour
{
    public ItemPickupSO itemData; //item data 
    public UnityEvent itemPickupEvent; //what happens when item is picked up?
    public ParticleSystem particles; //particles when item is picked up
    public GameObject itemUIImage; //UIImage when item is picked up
    public bool enableDestructionOnPickup = true; //destroy item when its picked up?
    public void OnItemPickup()
    {
        ParticleSystem particleObj = Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(particleObj, .25f);
    }
}
