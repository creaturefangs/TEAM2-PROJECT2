using UnityEngine;
using UnityEngine.Events;

public class ItemPickupComponent : MonoBehaviour
{
    public ItemPickupSO itemData;
    public UnityEvent itemPickupEvent;
    public ParticleSystem particles;
    
    public void OnItemPickup()
    {
        //ParticleSystem particleObj = Instantiate(particles, transform.position, Quaternion.identity);
        //Destroy(particleObj, .25f);
        Destroy(gameObject);
    }
}
