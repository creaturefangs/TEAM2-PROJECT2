using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Input = UnityEngine.Windows.Input;

public class Freeze : MonoBehaviour
{
    [SerializeField] private float freezeDistance; //How far away enemies can be to be frozen
    public bool canFreeze = false; //Can freeze?
    public float freezeTime = 5f; // The time the enemy remains slowed down in seconds
    public float slowSpeed = 0.5f; // The slowed-down speed
    public float slowAnimSpeed = 0.5f; // The slowed-down animation speed

    [SerializeField] private GameObject freezeParticles;
    private GameObject[] _enemyControllers;

    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame && canFreeze)
        {
            FreezeEnemies();
            Debug.Log("Freezing");
        }
    }

    private void FreezeEnemies()
    {
        if (canFreeze)
        {
            canFreeze = false;
            _enemyControllers = GameObject.FindGameObjectsWithTag("Enemy");
            List<GameObject> enemiesInArea = EnemiesInArea(_enemyControllers);

           
            foreach (var enemy in enemiesInArea)
            {
                IFreeze iFreeze = enemy.GetComponent<IFreeze>();
                if (iFreeze != null)
                {
                    // Apply freeze effect with desired parameters
                    iFreeze.ApplyFreeze(freezeTime, slowSpeed, slowAnimSpeed);

                    Vector3 freezePosition = new Vector3
                    (
                        enemy.transform.position.x,
                        enemy.transform.position.y + 1.0f,
                        enemy.transform.position.z
                    );
                    GameObject freeze = Instantiate(freezeParticles, freezePosition, Quaternion.identity);
                    freeze.transform.SetParent(enemy.transform);
                    Destroy(freeze, freezeTime);
                }
            }
        }
    }

    private List<GameObject> EnemiesInArea(GameObject[] enemies)
    {
        List<GameObject> enemiesInArea = new List<GameObject>();
        foreach (var enemy in _enemyControllers)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < freezeDistance)
            {
                enemiesInArea.Add(enemy);
            }
        }
        return enemiesInArea;
    }
}

