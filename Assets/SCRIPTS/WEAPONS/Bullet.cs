using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody bulletRigidbody;
    public BulletSO bullet;
    public Vector3 direction { get; set; }
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;
    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (bullet.isBullet)
        {
            bulletRigidbody.velocity = direction * bullet.bulletSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);

        ITakeDamage damageTaker = other.GetComponent<ITakeDamage>();
        //hit target
         Instantiate(damageTaker != null && !other.CompareTag("Enemy") ? vfxHitGreen : vfxHitRed, other.transform.position,
             Quaternion.identity);
        damageTaker?.TakeDamage(bullet.damage);
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);

        ITakeDamage damageTaker = other.gameObject.GetComponent<ITakeDamage>();
        //hit target
        Instantiate(damageTaker != null && !other.gameObject.CompareTag("Enemy") ? vfxHitGreen : vfxHitRed, other.transform.position,
            Quaternion.identity);
        damageTaker?.TakeDamage(bullet.damage);
    }
}
