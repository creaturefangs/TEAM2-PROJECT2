using UnityEngine;

[CreateAssetMenu (fileName = "Weapon", menuName = "Weapons/Weapon", order = 2)]
public class WeaponSO : ScriptableObject
{
    public GameObject weaponPrefab;
    public GameObject muzzleFlash;
    public BulletSO bullet;
    public string weaponName; //MOSTLY FOR DEBUG PURPOSES
    [Range(5, 31)]public int maxMagazineCapacity;
    [Range(2, 6)]public int timeBetweenShots;
    [Range(4, 11)] public int reloadTime;
}
