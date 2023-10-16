using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    public bool canBeOpened = false;
    private Animator _animator;
    private AudioSource _doorAudio;
    [SerializeField] private AudioClip unlockDoorClip;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        //_animator.enabled = false;

        _doorAudio = GetComponent<AudioSource>();
    }

    public void UnlockExitDoor()
    {
        //_animator.enabled = true;
        canBeOpened = true;

        _doorAudio.PlayOneShot(unlockDoorClip);
    }

    public void OpenExitDoor()
    {
        if (canBeOpened)
        {
            //Change to whatever the animator needs to open the door.
            _animator.SetTrigger("Open");
        }
    }
}
