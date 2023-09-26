using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Enemy", menuName = "Enemies/Enemy", order = 1)]
public class EnemySO : ScriptableObject
{
    public string name;
    public float maxHealth;
}
