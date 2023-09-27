using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    [Header("Weapons")]
    public WeaponSO weapon;
    public BulletSO bullet;

    [SerializeField] private Transform firePosition;

    [Header("References")]
    [SerializeField] private EnemyController enemyController;
    public void Shoot()
    {
        
    }

    public bool CanShoot(bool isFrozen)
    {
        if (!isFrozen)
        {
            return enemyController.distanceToPlayer <= enemyController.canShootDistance;
        }

        return false;
    }
}
