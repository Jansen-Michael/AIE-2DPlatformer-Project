using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private GameManager gm; // Reference to Game Manager

    void Start()
    {
        gm = FindObjectOfType<GameManager>();   // Set the reference to the game object
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() == true) // When Collide with player
        {
            gm.LevelComplete();     // Call Level Complete Function
        }
    }
}
