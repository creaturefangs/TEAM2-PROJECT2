using UnityEngine;

public class AlertTrigger : MonoBehaviour
{
    private AudioSource alertAudio;
    [SerializeField] private AudioClip intruderAlertClip;

    private void Start()
    {
        alertAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BoxCollider collider = GetComponent<BoxCollider>();
            collider.enabled = false;
            PlayAlertSound();
            Invoke(nameof(Destroy), 10.5f);
        }
    }

    private void PlayAlertSound()
    {
        alertAudio.PlayOneShot(intruderAlertClip);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
