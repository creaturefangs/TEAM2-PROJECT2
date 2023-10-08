using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex; // index of the checkpoint, will show to player when collided with.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.lastCheckpointPosition = this.transform.position;
            StartCoroutine(PlayerUI.instance.ShowCheckpointUnlock(checkpointIndex));
            Destroy(gameObject);
        }
    }
}
