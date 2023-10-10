using System.Collections;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex; // index of the checkpoint, will show to player when collided with.
    private float checkpointAlertDuration = 2.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.lastCheckpointPosition = this.transform.position;
            PlayerUI.instance.ShowCheckpointUnlock(checkpointIndex);

            StartCoroutine(DestroyCheckpointColliderAfterDelay());
            StartCoroutine(ClearCheckpointTextAfterDelay());
        }
    }

    private IEnumerator DestroyCheckpointColliderAfterDelay() 
    {
        yield return new WaitForSeconds(checkpointAlertDuration);
        Destroy(gameObject);
    }
    
    private IEnumerator ClearCheckpointTextAfterDelay()
    {
        yield return new WaitForSeconds(checkpointAlertDuration);
        PlayerUI.instance.checkpointAlertText.text = "";
    }
}