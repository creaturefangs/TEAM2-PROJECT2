using System;
using System.Collections;
using Invector;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public float chanceOutOfSight = 0.05f; // 5% chance to shoot when player is not in line of sight
    private float _lastCheckTime;
    public float checkInterval = 1.0f; // Check every second
    
    [Header("References")]
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private AudioSource audioSource;
    private EnemyHealth enemyHealth;
    
    

    private void Start()
    {
        _currentAmmo = weapon.maxMagazineCapacity;
        
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Update()
    {
        if (enemyHealth.isAlive && !enemyController.isFrozen)
        {
            if (Time.time >= _lastCheckTime + checkInterval && (enemyController.IsPlayerInLineOfSight() || Random.value < chanceOutOfSight))
            {
                _lastCheckTime = Time.time; // Update last check time

                if (enemyController.IsPlayerInRange(enemyController.canShootDistance))
                {
                    if (_timeSinceLastShot + weapon.timeBetweenShots <= Time.time)
                    {
                        // Delay between shots has passed, so shoot
                        Shoot();
                    }
                    // make enemy move while shooting
                    else
                    {
                        enemyController.currentEnemyState = EnemyController.EnemyStates.Walk;
                        enemyController.FindNextDestination();
                    }
                }
                else
                {
                    enemyController.currentEnemyState = EnemyController.EnemyStates.Walk;
                    enemyController.FindNextDestination();
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
        bool npcsHaveDespawned = false;
        if (_currentAmmo > 0 && Time.time >= _timeSinceLastShot + weapon.timeBetweenShots)
        {
            _timeSinceLastShot = Time.time;
            _currentAmmo--;
            isShooting = true;

            npcsHaveDespawned = true;
            
            // Calculate the direction to the player
            Vector3 direction = (enemyController.player.transform.position - transform.position).normalized;
            // Add random offset based on bullet spread
            direction += new Vector3(
                Random.Range(-weapon.bulletSpread, weapon.bulletSpread),
                Random.Range(-weapon.bulletSpread, weapon.bulletSpread),
                0);

            GameObject bulletObj = Instantiate(weapon.bullet.bulletPrefab, firePosition.position, firePosition.rotation);
            bulletObj.GetComponent<Bullet>().direction = direction;
            Destroy(bulletObj, weapon.reloadTime + .75f);
        
            GameObject flash = Instantiate(weapon.muzzleFlash, firePosition.position, firePosition.rotation);
            Destroy(flash, .5f);
            
            //Gunshot sound
            audioSource.PlayOneShot(weapon.gunshotSoundClip);
        }

        if (_currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }

        MakeNpcsFlee(npcsHaveDespawned);
    }


    private void MakeNpcsFlee(bool haveDespawned)
    {
        if (!haveDespawned)
        {
            NPCMovement[] npcs = GameObject.FindObjectsOfType<NPCMovement>();

            foreach (var npc in npcs)
            {
                npc.FleeFromGunFire();
            }
        }
    }

    private IEnumerator Reload()
    {
        enemyController.enemyAnimator.SetBool("isReloading", true);
        isReloading = true;
        //TODO: Start a reloading animation here
    
        yield return new WaitForSeconds(weapon.reloadTime);

        isReloading = false;
        _currentAmmo = weapon.maxMagazineCapacity;
        enemyController.enemyAnimator.SetBool("isReloading", false);

        //transition the enemy state back to Walk after reloading
        enemyController.currentEnemyState = EnemyController.EnemyStates.Walk;
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
