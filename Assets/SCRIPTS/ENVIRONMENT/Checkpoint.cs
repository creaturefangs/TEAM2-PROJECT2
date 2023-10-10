using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex; // index of the checkpoint, will show to player when collided with.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.lastCheckpointPosition = this.transform.position;
            PlayerUI.instance.ShowCheckpointUnlock(checkpointIndex);
            Invoke(nameof(ClearCheckpointText), 2.5f);
            Invoke(nameof(DestroyCheckpointCollider), .1f);
        }
    }

    private void DestroyCheckpointCollider()
    {
        Destroy(gameObject);
    }
    
    private void ClearCheckpointText()
    {
        PlayerUI.instance.checkpointAlertText.text = "";
    }
}
