using System;
using System.Collections;
using Invector;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    [Header("Weapons")]
    public WeaponSO weapon;
    private int _currentAmmo;
    private float _timeSinceLastShot;
    [SerializeField] private Transform firePosition;
    public float lookRotationSpeed = 5.0f;
    private bool _isReloading = false;
    private bool _isShooting = false;
    
    [Header("References")]
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private LayerMask playerMask;

    private void Start()
    {
        _currentAmmo = weapon.maxMagazineCapacity;
    }

    private void Update()
    {
        if (enemyController.isAlive && !enemyController.isFrozen)
        {
            if (enemyController.IsPlayerInRange(enemyController.canShootDistance))
            {
                if (_timeSinceLastShot + weapon.timeBetweenShots <= Time.time)
                {
                    // Delay between shots has passed, so shoot
                    Shoot();
                }
            }
        }

        if (_isShooting)
        {
            RotateAroundPlayer();
        }
    }


    public void Shoot()
    {
        if (_currentAmmo > 0 && _timeSinceLastShot < Time.time + weapon.timeBetweenShots)
        {
            _timeSinceLastShot = Time.time;
            _currentAmmo--;
            _isShooting = true;
            
            GameObject flash = Instantiate(weapon.muzzleFlash, firePosition.position, firePosition.rotation);
            Destroy(flash, .5f);
            
            Instantiate(weapon.bullet.bulletPrefab, firePosition.position, firePosition.rotation);
            ShootingRaycast();
        }

        if (_currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }
    }

    private void ShootingRaycast()
    {
        if (Physics.Raycast(
                enemyController.enemyForwardVector.position,
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
                    damageTaker.TakeDamage(weapon.bullet.damage);
                    Debug.Log("Hit" + hitInfo.collider.name);
                }
            }
        }
    }
    

    private IEnumerator Reload()
    {
        _isReloading = true;
        //Start a reloading animation here
        
        yield return new WaitForSeconds(weapon.reloadTime);
        
        _isReloading = false;
        _currentAmmo = weapon.maxMagazineCapacity;
    }

    private void RotateAroundPlayer()
    {
        // Calculate the direction to the player
        Vector3 direction = (enemyController.player.transform.position - transform.position).normalized;

        // Update the enemy's rotation to look at the player
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
    }
}
