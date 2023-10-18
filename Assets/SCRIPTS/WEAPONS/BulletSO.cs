using UnityEngine;

[CreateAssetMenu (fileName = "Bullet", menuName = "Bullets/Bullet", order = 3)]
public class BulletSO : ScriptableObject
{
    public bool isBullet = true; // is this bullet actually a bullet? mostly for flamethrower
    public GameObject bulletPrefab;
    [Range(3, 21)]public float damage;
    [Range(30, 50)]public float maxHitDistance;
    [Range(50, 120)] public float bulletSpeed;
}
