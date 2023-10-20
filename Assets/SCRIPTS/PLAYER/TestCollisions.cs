using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollisions : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collided with " + other.gameObject.name);
    }
}
