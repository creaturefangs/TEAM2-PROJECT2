using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Input = UnityEngine.Windows.Input;

public class Freeze : MonoBehaviour
{
    [SerializeField] private float freezeDistance;
    
    public bool canFreeze = false;
    public int freezeItemCount = 0;
    public bool _canMakeFreeze = false;

    private GameObject[] _enemyControllers;
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (Keyboard.current.fKey.wasPressedThisFrame && canFreeze)
        // {
        //     FreezeEnemies();
        // }
    }

    public void FreezeEnemies()
    {
        if (canFreeze)
        {
            canFreeze = false;
            _enemyControllers = GameObject.FindGameObjectsWithTag("Enemy");

            if (EnemiesInArea(_enemyControllers))
            {
                //_enemyControllers.Freeze();
            }

        }
    }

    private bool EnemiesInArea(GameObject[] enemies)
    {
        foreach (var enemy in _enemyControllers)
        {
            return Vector3.Distance(transform.position, enemy.transform.position) < freezeDistance;
        }
        return false;
    }
    
    public void CraftFreeze()
    {
        if (_canMakeFreeze)
        {
            freezeItemCount = 0;
            _canMakeFreeze = false;
            canFreeze = true;
            PlayerUI.instance.EnableUIElement(PlayerUI.instance.freezeImage);
        }
    }
}
