using UnityEngine;

[CreateAssetMenu (fileName = "Enemy", menuName = "Enemies/Enemy", order = 1)]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public float maxHealth;
}
