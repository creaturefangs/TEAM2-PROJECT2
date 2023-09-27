using UnityEngine;

[CreateAssetMenu (fileName = "Bullet", menuName = "Bullets/Bullet", order = 3)]
public class BulletSO : ScriptableObject
{
    public float damage;
    public float maxHitDistance;
    public float bulletSpeed;
    public ParticleSystem bulletParticles;
}
