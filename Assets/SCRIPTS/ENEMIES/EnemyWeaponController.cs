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
#pragma warning disable CS0414 // Field is assigned but its value is never used
    private bool isReloading = false;
#pragma warning restore CS0414 // Field is assigned but its value is never used
    private bool isShooting = false;
    
    [Header("References")]
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private AudioSource audioSource;

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

        if (isShooting)
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
            isShooting = true;
            
            // Calculate the direction to the player
            Vector3 direction = (enemyController.player.transform.position - transform.position).normalized;
            // Add random offset based on accuracy radius
            direction += new Vector3(UnityEngine.Random.Range(-weapon.bulletSpread, weapon.bulletSpread), 
                UnityEngine.Random.Range(-weapon.bulletSpread, weapon.bulletSpread), 0);

            GameObject bulletObj = Instantiate(weapon.bullet.bulletPrefab, firePosition.position, firePosition.rotation);
            bulletObj.GetComponent<Bullet>().direction = direction;
            Destroy(bulletObj, weapon.reloadTime + .75f);
            
            GameObject flash = Instantiate(weapon.muzzleFlash, firePosition.position, firePosition.rotation);
            Destroy(flash, .5f);
        }

        if (_currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }
    }
    

    private IEnumerator Reload()
    {
        isReloading = true;
        //TODO: Start a reloading animation here
        
        yield return new WaitForSeconds(weapon.reloadTime);
        
        isReloading = false;
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
