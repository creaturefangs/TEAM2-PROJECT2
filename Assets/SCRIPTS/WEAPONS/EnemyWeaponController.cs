using System;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    [Header("Weapons")]
    public WeaponSO weapon;
    private int _currentAmmo;
    private float _timeSinceLastShot;
    [SerializeField] private Transform firePosition;
    public float lookRotationSpeed = 5.0f;
    
    [Header("References")]
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private LayerMask playerMask;

    private void Start()
    {
        _currentAmmo = weapon.maxMagazineCapacity;
    }

    private void Update()
    {
        if (Time.time > _timeSinceLastShot + weapon.timeBetweenShots && CanShoot(enemyController.isFrozen))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (_currentAmmo > 0 && _timeSinceLastShot < Time.time + weapon.timeBetweenShots)
        {
            _timeSinceLastShot = Time.time;
            _currentAmmo--;
            Instantiate(weapon.muzzleFlash, firePosition.position, firePosition.rotation);
            Instantiate(weapon.bullet.bulletPrefab, firePosition.position, firePosition.rotation);
            ShootingRaycast();

            Vector3 direction = (enemyController.player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            //Smoothly rotate towards the player, rotation speed is controlled by lookRotationSpeed
            transform.rotation = 
                Quaternion.Slerp(transform.rotation,
                    lookRotation,
                    Time.deltaTime * lookRotationSpeed);

        }
    }

    private void ShootingRaycast()
    {
        if (Physics.Raycast(
                firePosition.transform.position,
                firePosition.forward,
                out RaycastHit hitInfo,
                weapon.bullet.maxHitDistance,
                playerMask))
        {
            if (hitInfo.collider == null) return;
            if (hitInfo.collider.GetComponent<ITakeDamage>() != null)
            {
                ITakeDamage damageTaker = hitInfo.collider.GetComponent<ITakeDamage>();
                if (damageTaker != null)// just to make sure damage taker is still there
                {
                    damageTaker.TakeDamage();
                    Debug.Log("Hit" + hitInfo.collider.name);
                }
            }
        }
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
