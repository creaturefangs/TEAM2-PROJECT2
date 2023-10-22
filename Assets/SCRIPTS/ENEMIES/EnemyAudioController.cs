using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAudioController : MonoBehaviour
{
    public AudioSource enemyMainAudio;
    public AudioSource deathAudio;
    public AudioClip[] deathClips;

    public AudioClip GetRandomEnemyAudioClip(AudioClip[] clips)
    {
        return clips[Random.Range(0, clips.Length)];
    }
    
    public void PlayAudioClip(AudioSource source, AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}
