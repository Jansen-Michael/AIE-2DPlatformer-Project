using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Movement
    public float speed;
    public float minSpeed = 10;
    public float maxSpeed = 25;
    public float timeFromZeroToMax = 1.5f;
    public float timeFromMaxToZero = 0.75f;
    private float moveInput;

    //Speed Boost
    public float speedBoost = 35f;
    private float speedBoostTimeCounter;
    public float speedBoostTime = 3f;
    public bool activateSpeedBoost = false;

    private PlayerController player;
    private Rigidbody2D rb;
    private Animator animator;
    private bool facingRight = true;

    // Jump
    public float jumpForce = 20f;
    public bool doubleJump = true;
    public float doubleJumpForce = 40f;
    private bool canDoubleJump = false;
    private float jumpTimeCounter;
    public float jumpTime;
    public bool isfirstJump;

    // Wall Jump
    public float xWallJumpForce = 40f;
    public float yWallJumpForce = 40f;
    public float wallJumpTime = 0.5f;
    private float wallJumpTimer;
    public float wallSlidingSpeed = 15f;
    private bool isWallSliding;
    public bool isWallJumping;

    public enum WallJumpDirection { None, FromLeft, FromRight };           // Enumerator type used to determine the dash direction
    private WallJumpDirection wallJumpDirection;                                // This is where we store the actual dash's direfction

    public GameObject spriteBody;

    void Start()
    {
        // Grab Components and reset variables
        player = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        speedBoostTimeCounter = speedBoostTime;
        wallJumpDirection = WallJumpDirection.None;
    }

    private void FixedUpdate()
    {
        if (player.canMove == false) { return; } // Disable allow forms of movement
        //Wall Jump
        if (isWallSliding)  // Check if we should wall slide
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        if (isWallJumping == true)
        {
            wallJumpTimer -= Time.fixedDeltaTime;
            if (wallJumpDirection == WallJumpDirection.FromLeft)
            {
                rb.velocity = new Vector2(xWallJumpForce, yWallJumpForce);
            }
            else if (wallJumpDirection == WallJumpDirection.FromRight)
            {
                rb.velocity = new Vector2(-xWallJumpForce, yWallJumpForce);
            }


        }
        
        if (isWallJumping == true) { return; }
        // Movement
        speed = Mathf.Clamp(speed, minSpeed, speedBoost);
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    public void Jump()
    {
        if (player.CheckGrounded() == true)
        {
            jumpTimeCounter = jumpTime;
        }

        if (Input.GetButtonDown("Jump") && player.CheckGrounded() == true)
        {
            isfirstJump = true;
            rb.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetButton("Jump") && isfirstJump == true)
        {

            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isfirstJump = false;
            }
        }

        if (Input.GetButtonUp("Jump")) { isfirstJump = false; }
    }

    public void WallJump()
    {
        if (player.CheckGrounded()) { wallJumpDirection = WallJumpDirection.None; }

        if ((player.CheckLeftWall() || player.CheckRightWall()) && player.CheckGrounded() == false && moveInput != -0)
        {
            isWallSliding = true;
        } else {
            isWallSliding = false;
        }

        if (wallJumpTimer <= 0) { isWallJumping = false; wallJumpTimer = wallJumpTime; }
        else
        {
            if (Input.GetButtonDown("Jump") && isWallSliding == true && moveInput == -1) // if pushing against wall and jump is pressed
            {
                //print("From left");
                wallJumpDirection = WallJumpDirection.FromLeft;     // Set isWallJumping to from left
                isWallJumping = true;
                Flip();
            }
            else if (Input.GetButtonDown("Jump") && isWallSliding == true && moveInput == 1) // if pushing against wall and jump is pressed
            {
                //print("From right");
                rb.velocity = new Vector2(-xWallJumpForce * 2, xWallJumpForce);
                wallJumpDirection = WallJumpDirection.FromRight;    // Set isWallJumping to from right
                isWallJumping = true;
                Flip();
            }
        } 
    }

    public void DoubleJump()
    {
        if (player.CheckGrounded() == true)
        {
            canDoubleJump = true;       // Enable double jump
        }

        if (Input.GetButtonDown("Jump") && player.CheckGrounded() == false && isfirstJump == false && canDoubleJump == true && isWallJumping == false)
        {
            rb.velocity = Vector2.up * doubleJumpForce;     // Perform double jump
            canDoubleJump = false;                          // Disable double jump
        }
    }

    public void Movement()
    {
        // Set Acceleration
        float acceleration = timeFromZeroToMax * Time.deltaTime;
        float decceleration = timeFromMaxToZero * Time.deltaTime;

        moveInput = Input.GetAxisRaw("Horizontal");

        if (activateSpeedBoost == false)
        {
            if (moveInput != 0) { speed = Mathf.MoveTowards(speed, maxSpeed, acceleration); }
            else { speed = Mathf.MoveTowards(speed, minSpeed, decceleration); }
        }
        else
        {
            SpeedBoost(acceleration, decceleration);
        }


        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }
    }

    void SpeedBoost(float acceleration, float deceleration)
    {
        // Note: Change to while loop
        if (speedBoostTimeCounter > 0)
        {
            if (moveInput != 0) { speed = Mathf.MoveTowards(speed, speedBoost, acceleration * 2); }
            else { speed = Mathf.MoveTowards(speed, maxSpeed, deceleration / 2); }
            speedBoostTimeCounter -= Time.deltaTime;
            animator.SetBool("isSpeedBoost", true);
        }
        else if (player.CheckGrounded() == true)
        {
            speedBoostTimeCounter = speedBoostTime;
            activateSpeedBoost = false;
            animator.SetBool("isSpeedBoost", false);
        }
    }

    public void KillSpeed()
    {
        if (activateSpeedBoost) { speed = maxSpeed; } 
        else { speed = minSpeed; }
        rb.velocity = Vector2.zero;         // Stop all velocity
        rb.gravityScale = 9;                // Restore Gravity
    }

    void Flip() // Flips the player's sprite in the opposite direction
    {
        facingRight = !facingRight;
        Vector3 scaler = spriteBody.transform.localScale;
        scaler.x *= -1;
        spriteBody.transform.localScale = scaler;
    }
}
