using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _doorAudio;
    [SerializeField] private AudioClip unlockDoorClip;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _doorAudio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        // Subscribe to the event when the script is enabled.
        GameManager.OnPlayerReached9Kills += OpenExitDoor;
    }

    private void OnDisable()
    {
        // Unsubscribe from the event when the script is disabled to prevent memory leaks.
        GameManager.OnPlayerReached9Kills -= OpenExitDoor;
    }

    public void OpenExitDoor()
    {
        // Implement the logic to open the door (e.g., set a trigger in the animator).
        _animator.SetTrigger("Open");
        _doorAudio.PlayOneShot(unlockDoorClip);
    }
}