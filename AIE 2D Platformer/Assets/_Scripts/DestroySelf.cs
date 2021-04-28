using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public float timeBeforeDestroy = 2f;        // Destroy Timer

    void Start()
    {
        Destroy(gameObject, timeBeforeDestroy); // Destroy Self after a certain amount of time.
    }
}
