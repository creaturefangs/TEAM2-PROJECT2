using UnityEngine;

[CreateAssetMenu (fileName = "Weapon", menuName = "Weapons/Weapon", order = 2)]
public class WeaponSO : ScriptableObject
{
    public GameObject weaponPrefab; //only needed for instantiation, for example if enemies should have a random weapon on start
    public GameObject muzzleFlash;
    public BulletSO bullet;
    public string weaponName; //MOSTLY FOR DEBUG PURPOSES
    public AudioClip gunshotSoundClip;
    [Range(5, 31)] public int maxMagazineCapacity;
    [Range(.5f, 6f)] public float timeBetweenShots;
    [Range(1.5f, 3.5f)] public float reloadTime;
    
    [Tooltip("The deviation range for weapon accuracy")]
    public float bulletSpread;
}
