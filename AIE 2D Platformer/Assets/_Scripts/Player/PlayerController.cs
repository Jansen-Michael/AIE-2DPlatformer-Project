using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // References to other components and GameObjects
    private GameManager gm;
    private PlayerMovement playerMovement;
    private DashMove dashMove;
    private Animator animator;
    private Rigidbody2D rb;

    // Check is grounded variables
    public Transform groundCheckBox;
    public Vector2 groundCheckRadius;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlatform;

    // Check is the wall variables
    public Transform leftWallCheckBox;
    public Transform rightWallCheckBox;
    public Vector2 wallCheckRadius;
    public LayerMask whatIsWall;

    public bool canMove = true;
    public bool dashEnabled = true;
    public bool doubleJumpEnabled = true;

    // Boomerang
    private Boomerang boomerang;
    public GameObject boomerangChargeMeter;
    public GameObject chargeBar;
    public float timeToMaxCharge = 1.2f;
    private float chargeTime;
    private float chargePercentage;

    void Start()
    {
        // Find and get required components
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        boomerang = FindObjectOfType<Boomerang>().GetComponent<Boomerang>();
        playerMovement = GetComponent<PlayerMovement>();
        dashMove = GetComponent<DashMove>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Set up variables
        gm.lastCheckPointPos = transform.position;  // Set spawn point
        boomerangChargeMeter.SetActive(false);                 // Hide the boomerang charge meter
        canMove = true;                             // Allow movement
    }

    void Update()
    {
        if (canMove == false)   // Disable movement and inputs and animation
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isFirstJump", false);
            animator.SetBool("isDoubleJump", false);
            animator.SetBool("isInAir", false);
            animator.SetBool("isThrowing", false);
            animator.SetBool("isDashing", false);
            return; 
        }

        playerMovement.Movement();
        dashMove.Dash();
        playerMovement.Jump();
        playerMovement.WallJump();
        playerMovement.DoubleJump();
        ActivateBoomerang();
    }

    private void ActivateBoomerang()
    {
        Vector2 mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 throwDirection = new Vector2(mouseLocation.x - transform.position.x, mouseLocation.y - transform.position.y).normalized;

        if (boomerang.isWithPlayer())
        {
            if (Input.GetMouseButton(0))
            {
                chargeTime += Time.deltaTime;
                boomerangChargeMeter.SetActive(true);
                boomerangChargeMeter.transform.right = throwDirection;
                chargePercentage = chargeTime / timeToMaxCharge;
                chargePercentage = Mathf.Clamp01(chargePercentage);
                chargeBar.transform.localScale = new Vector3(chargePercentage, chargeBar.transform.localScale.y);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                boomerangChargeMeter.SetActive(false);
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
        if (collision.gameObject.CompareTag("Death"))   // If Collide with gameobject of tag Death
        {
            gm.RespawnPlayer();     // Respawn Player
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Speed-Power-Up")) // If collide with speed power-up
        {
            playerMovement.SpeedPowerUp(true);      // Enable speed boost
            Destroy(collision);                     // Destroy Power-up
            //collision.gameObject.SetActive(false);  // Deactivate power-up
        }
        if (collision.CompareTag("Time-Power-Up"))
        {
            gm.AddTime(30f);                        // Add 30 seconds to the timer
            Destroy(collision);                     // Destroy Power-up
        }
        if (collision.GetComponent<Coin>() == true)
        {
            collision.GetComponent<Coin>().Collected();
        }
    }

    public bool CheckGrounded()     // Check if there are gameobjects with the matching layer on the check radius
    {
        Collider2D groundCheck1 = Physics2D.OverlapBox(groundCheckBox.position, groundCheckRadius, 0f, whatIsGround);
        Collider2D groundCheck2 = Physics2D.OverlapBox(groundCheckBox.position, groundCheckRadius, 0f, whatIsPlatform);

        if ((groundCheck1 || groundCheck2) && rb.velocity.y == 0)   // Check if player contact with matching layer and not in air
        {
            return true;
        } 
        else
        {
            return false;
        }
    }

    public bool CheckLeftWall()     // Check if there are gameobjects with the matching layer on the check radius
    {
        Collider2D leftWallCheck1 = Physics2D.OverlapBox(leftWallCheckBox.position, wallCheckRadius, 0f, whatIsGround);
        Collider2D leftWallCheck2 = Physics2D.OverlapBox(leftWallCheckBox.position, wallCheckRadius, 0f, whatIsWall);

        if (leftWallCheck1 || leftWallCheck2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckRightWall()    // Check if there are gameobjects with the matching layer on the check radius
    {
        Collider2D rightWallCheck1 = Physics2D.OverlapBox(rightWallCheckBox.position, wallCheckRadius, 0f, whatIsGround);
        Collider2D rightWallCheck2 = Physics2D.OverlapBox(rightWallCheckBox.position, wallCheckRadius, 0f, whatIsWall);

        if (rightWallCheck1 || rightWallCheck2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos() // Draw Gizmos for ground check and wall checks
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckBox.position, groundCheckRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(leftWallCheckBox.position, wallCheckRadius);
        Gizmos.DrawWireCube(rightWallCheckBox.position, wallCheckRadius);
    }
}
