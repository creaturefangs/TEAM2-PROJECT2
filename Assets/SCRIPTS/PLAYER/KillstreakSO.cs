using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Killstreaks", menuName = "Killstreaks/Killstreak", order = 5)]
public class KillstreakSO : ScriptableObject
{
    public ParticleSystem killStreakParticles;
    public int damageMultiplier;
    public int additionalHealth;
}
