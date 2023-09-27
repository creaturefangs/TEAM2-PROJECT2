using UnityEngine;

[CreateAssetMenu (fileName = "Bullet", menuName = "Bullets/Bullet", order = 3)]
public class BulletSO : ScriptableObject
{
    public GameObject bulletPrefab;
    [Range(7, 21)]public float damage;
    [Range(30, 50)]public float maxHitDistance;
    [Range(50, 120)] public float bulletSpeed;
}
