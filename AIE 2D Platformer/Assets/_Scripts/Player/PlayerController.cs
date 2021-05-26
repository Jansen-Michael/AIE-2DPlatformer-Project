﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager gm;
    private PlayerMovement playerMovement;
    private DashMove dashMove;

    private Rigidbody2D rb;

    public Transform groundCheck;
    public Vector2 groundCheckRadius;
    public LayerMask whatIsGround;

    public Transform leftWallCheck;
    public Transform rightWallCheck;
    public Vector2 wallCheckRadius;
    public LayerMask whatIsWall;

    public bool canMove = true;
    public bool dashEnabled = true;
    public bool doubleJumpEnabled = true;
    private float moveInput;
    private bool jumpInput;

    // Boomerang
    private Boomerang boomerang;
    public GameObject chargeBar;
    public float timeToMaxCharge = 1.2f;
    private float chargeTime;
    private float chargePercentage;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        boomerang = GameObject.FindGameObjectWithTag("Boomerang").GetComponent<Boomerang>();
        playerMovement = GetComponent<PlayerMovement>();
        dashMove = GetComponent<DashMove>();
        rb = GetComponent<Rigidbody2D>();

        gm.lastCheckPointPos = transform.position;
        canMove = true;
    }

    void Update()
    {
        if (canMove == false) { return; }

        jumpInput = Input.GetButtonDown("Jump");
        playerMovement.Movement();
        if (dashEnabled) { dashMove.Dash(); }
        playerMovement.Jump();
        playerMovement.WallJump();
        if (doubleJumpEnabled) { playerMovement.DoubleJump(jumpInput); }
        ActivateBoomerang();
    }

    private void ActivateBoomerang()
    {
        Vector2 mouseLocation = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

        if (boomerang.isWithPlayer())
        {
            if (Input.GetMouseButton(0))
            {
                chargeTime += Time.deltaTime;
                chargePercentage = chargeTime / timeToMaxCharge;
                chargePercentage = Mathf.Clamp01(chargePercentage);
                chargeBar.transform.localScale = new Vector3(chargePercentage, chargeBar.transform.localScale.y);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Vector2 throwDirection = new Vector2(mouseLocation.x - transform.position.x, mouseLocation.y - transform.position.y).normalized;
                boomerang.ThrowBoomerang(throwDirection, chargePercentage);
                chargeTime = 0;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (!boomerang.isWithPlayer()) { boomerang.FreezeBoomerang(); }
        }
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

    public bool CheckGrounded()
    {
        if(Physics2D.OverlapBox(groundCheck.position, groundCheckRadius, 0f, whatIsGround))
        {
            return true;
        } 
        else
        {
            return false;
        }
    }

    public bool CheckLeftWall()
    {
        if (Physics2D.OverlapBox(leftWallCheck.position, wallCheckRadius, 0f, whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckRightWall()
    {
        if (Physics2D.OverlapBox(rightWallCheck.position, wallCheckRadius, 0f, whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(leftWallCheck.position, wallCheckRadius);
        Gizmos.DrawWireCube(rightWallCheck.position, wallCheckRadius);
    }
}
