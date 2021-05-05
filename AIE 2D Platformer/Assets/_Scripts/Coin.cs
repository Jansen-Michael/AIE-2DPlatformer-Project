using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int pointValue;              // Value of the coin
    private GameManager gm;  // Reference to the score

    void Start()
    {
        gm = FindObjectOfType<GameManager>();    // Set the score reference
    }

    public void Collected()
    {
        gm.AddScore(pointValue);  // Add point value to the score
        Destroy(gameObject);                // Destroy self after being collected
    }
}
