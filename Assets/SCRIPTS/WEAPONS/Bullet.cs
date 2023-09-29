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
        bulletRigidbody.velocity = direction * bullet.bulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);

        ITakeDamage damageTaker = other.GetComponent<ITakeDamage>();
        //hit target
        // Instantiate(damageTaker != null ? vfxHitGreen : vfxHitRed, other.transform.position,
        //     Quaternion.identity);
        damageTaker?.TakeDamage(bullet.damage);
    }
}
