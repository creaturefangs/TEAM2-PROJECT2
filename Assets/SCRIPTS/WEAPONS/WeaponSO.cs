using UnityEngine;

[CreateAssetMenu (fileName = "Weapon", menuName = "Weapons/Weapon", order = 2)]
public class WeaponSO : ScriptableObject
{
    public GameObject weaponPrefab;
    public string weaponName;
    public int maxMagazineCapacity;
    public int timeBetweenShots;
}
