using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAudioController : MonoBehaviour
{
    [HideInInspector] public AudioSource enemyAudio;

    public AudioClip[] deathClips;
    private void Awake()
    {
        enemyAudio = GetComponent<AudioSource>();
    }

    public AudioClip GetRandomEnemyAudioClip(AudioClip[] clips)
    {
        return clips[Random.Range(0, clips.Length)];
    }
    
    public void PlayAudioClip(AudioClip clip)
    {
        enemyAudio.PlayOneShot(clip);
    }
}
