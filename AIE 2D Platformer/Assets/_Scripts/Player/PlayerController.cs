using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager gm;
    private PlayerMovement playerMovement;
    private DashMove dashMove;

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
        rb = GetComponent<Rigidbody2D>();

        // Set up variables
        gm.lastCheckPointPos = transform.position;  // Set spawn point
        boomerangChargeMeter.SetActive(false);                 // Hide the boomerang charge meter
        canMove = true;                             // Allow movement
    }

    void Update()
    {
        if (canMove == false) { return; }

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
        if (collision.gameObject.CompareTag("Death"))
        {
            gm.RespawnPlayer();
            //print("Respawn");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Speed-Power-Up"))
        {
            playerMovement.SpeedPowerUp(true);
            collision.gameObject.SetActive(false);
        }
        if (collision.CompareTag("Time-Power-Up"))
        {
            gm.AddTime(30f);
            collision.gameObject.SetActive(false);
        }
        if (collision.GetComponent<Coin>() == true)
        {
            collision.GetComponent<Coin>().Collected();
        }
    }

    public bool CheckGrounded()
    {
        Collider2D groundCheck1 = Physics2D.OverlapBox(groundCheckBox.position, groundCheckRadius, 0f, whatIsGround);
        Collider2D groundCheck2 = Physics2D.OverlapBox(groundCheckBox.position, groundCheckRadius, 0f, whatIsPlatform);

        if (groundCheck1 || groundCheck2)
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

    public bool CheckRightWall()
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
