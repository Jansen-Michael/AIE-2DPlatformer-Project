using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager gm;
    private PlayerMovement playerMovement;
    private DashMove dashMove;

    public bool canMove = true;

    public bool dashEnabled = true;
    public bool doubleJumpEnabled = true;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        playerMovement = GetComponent<PlayerMovement>();
        dashMove = GetComponent<DashMove>();

        gm.lastCheckPointPos = transform.position;
        canMove = true;
    }

    void Update()
    {
        if (canMove == false) { return; }

        if (dashEnabled) { dashMove.Dash(); }
        playerMovement.Jump(doubleJumpEnabled);
    }

    private void FixedUpdate()
    {
        if (canMove == false) { return; }

        playerMovement.Movement();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            gm.RespawnPlayer();
            print("Respawn");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Power-Up"))
        {
            playerMovement.activateSpeedBoost = true;
        }
        if (collision.GetComponent<Coin>() == true)
        {
            collision.GetComponent<Coin>().Collected();
        }
    }
}
