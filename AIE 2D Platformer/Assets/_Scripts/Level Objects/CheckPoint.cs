using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private GameManager gm; // reference to GameManager
    public Sprite disableCheckPoint;
    public Sprite enabledCheckPoint;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();    // Set reference to the GameManager
        GetComponent<SpriteRenderer>().sprite = disableCheckPoint;                  // Set Sprite to disabled checkpoint
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>()) // When contact with player
        {
            gm.lastCheckPointPos = transform.position;                  // Set GameManagers last check point position to this check point position
            GetComponent<SpriteRenderer>().sprite = enabledCheckPoint;  // Set sprite to enabled check point
        }
        if (collision.GetComponent<Boomerang>())        // When contact with Boomerang
        {
            gm.lastCheckPointPos = transform.position;                  // Set GameManagers last check point position to this check point position
            GetComponent<SpriteRenderer>().sprite = enabledCheckPoint;  // Set sprite to enabled check point
        }
    }
}
