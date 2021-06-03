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
    private bool shouldDecelerate = true;
    private float moveInput;

    //Speed Boost
    public float speedBoost = 35f;
    private float speedBoostTimeCounter;
    public float wallJumpSpeedBoostTime = 1f;
    public float powerupSpeedBoost = 3f;
    private bool activateSpeedBoost = false;

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
    public float jumpTime = 1.8f;
    private bool isfirstJump;

    // Wall Jump
    public float xWallJumpForce = 40f;
    public float yWallJumpForce = 40f;
    private float yWallJump;
    public float wallJumpTime = 0.5f;
    private float wallJumpTimer;
    public float wallSlidingSpeed = 15f;
    private bool isWallSliding;
    private bool isWallJumping;

    public enum WallJumpDirection { None, FromLeft, FromRight };           // Enumerator type used to determine the dash direction
    private WallJumpDirection wallJumpDirection;                                // This is where we store the actual dash's direfction

    public GameObject spriteBody;

    void Start()
    {
        // Grab Components and reset variables
        player = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        yWallJump = yWallJumpForce;
        wallJumpDirection = WallJumpDirection.None;
    }

    private void FixedUpdate()
    {
        if (player.canMove == false) { return; } // Disable allow forms of movement

        // Wall Slide
        if (isWallSliding)  // Check if we should wall slide
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));    // Reduce fall speed to emulated clinging on a wall
        }

        // Wall Jump
        if (isWallJumping == true)
        {
            wallJumpTimer -= Time.fixedDeltaTime;   // Decrase wall jump timer
            shouldDecelerate = false;               // Prevent Deceleration
            if (wallJumpDirection == WallJumpDirection.FromLeft)
            {
                rb.velocity = new Vector2(xWallJumpForce, yWallJump);   // Wall jump to the right
            }
            else if (wallJumpDirection == WallJumpDirection.FromRight)
            {
                rb.velocity = new Vector2(-xWallJumpForce, yWallJump);  // Wall jump to the left
            }
        }
        
        if (isWallJumping == true) { return; } // Disable movement if currently wall jumping
        // Movement
        moveInput = Input.GetAxisRaw("Horizontal"); // Get direction input
        speed = Mathf.Clamp(speed, minSpeed, speedBoost);
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        shouldDecelerate = true;
    }

    public void Jump()
    {
        if (player.CheckGrounded() == true)
        {
            jumpTimeCounter = jumpTime;
        }

        if (Input.GetButtonDown("Jump") && player.CheckGrounded() == true)  // If grounded and space bar is pressed than jump
        {
            isfirstJump = true; // set isfirst jump to true
            rb.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetButton("Jump") && isfirstJump == true) // Allow for dynamic jump based on how long player holds the button
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;   // Continue to raise player position
                jumpTimeCounter -= Time.deltaTime;      // Reduce jump timer
            }
            else
            {
                isfirstJump = false; // set isfirst jump to false
            }
        }

        if (Input.GetButtonUp("Jump")) { isfirstJump = false; } // if let go of space bar than set isfirst jump to false
    }

    public void WallJump()
    {
        if (player.CheckGrounded()) { wallJumpDirection = WallJumpDirection.None; } // If grounded reset wall jump

        if ((player.CheckLeftWall() || player.CheckRightWall()) && player.CheckGrounded() == false && moveInput != -0)  // Check if we should be wall sliding
        {
            isWallSliding = true;
        } else {
            isWallSliding = false;
        }

        if (wallJumpTimer <= 0) { isWallJumping = false; wallJumpTimer = wallJumpTime; }    // if wall jump is done set isWallJumping to false and reset wall jump timer
        else
        {
            switch (wallJumpDirection)
            {
                case (WallJumpDirection.None):
                    if (Input.GetButtonDown("Jump") && isWallSliding == true && moveInput == -1) // if pushing against wall and jump is pressed
                    {
                        speedBoostTimeCounter = wallJumpSpeedBoostTime;     // Set Speed boost timer
                        activateSpeedBoost = true;                          // Set Speed boost to true
                        yWallJump = yWallJumpForce;                         // Set Vertical jump Force to original value
                        wallJumpDirection = WallJumpDirection.FromLeft;     // Set isWallJumping to from left
                        isWallJumping = true;
                        Flip();
                    }
                    else if (Input.GetButtonDown("Jump") && isWallSliding == true && moveInput == 1) // if pushing against wall and jump is pressed
                    {
                        speedBoostTimeCounter = wallJumpSpeedBoostTime;     // Set Speed boost timer
                        activateSpeedBoost = true;                          // Set Speed boost to true
                        yWallJump = yWallJumpForce;                         // Set Vertical jump Force to original value
                        wallJumpDirection = WallJumpDirection.FromRight;    // Set isWallJumping to from right
                        isWallJumping = true;
                        Flip();
                    }
                    break;

                case (WallJumpDirection.FromLeft):      // If we wall jump from same direction reduce value if opposite direction give speed boost
                    if (Input.GetButtonDown("Jump") && isWallSliding == true && moveInput == -1) // if pushing against wall and jump is pressed
                    {
                        yWallJump = yWallJump / 2;               // Cut Jump height in half
                        wallJumpDirection = WallJumpDirection.FromLeft;     // Set isWallJumping to from left
                        isWallJumping = true;
                        Flip();
                    }
                    else if (Input.GetButtonDown("Jump") && isWallSliding == true && moveInput == 1) // if pushing against wall and jump is pressed
                    {
                        speedBoostTimeCounter = wallJumpSpeedBoostTime;     // Set Speed boost timer
                        activateSpeedBoost = true;                          // Set Speed boost to true
                        yWallJump = yWallJumpForce;                         // Set Vertical jump Force to original value
                        wallJumpDirection = WallJumpDirection.FromRight;    // Set isWallJumping to from right
                        isWallJumping = true;
                        Flip();
                    }
                    break;

                case (WallJumpDirection.FromRight):     // If we wall jump from same direction reduce value if opposite direction give speed boost
                    if (Input.GetButtonDown("Jump") && isWallSliding == true && moveInput == -1) // if pushing against wall and jump is pressed
                    {
                        speedBoostTimeCounter = wallJumpSpeedBoostTime;     // Set Speed boost timer
                        activateSpeedBoost = true;                          // Set Speed boost to true
                        yWallJump = yWallJumpForce;                         // Set Vertical jump Force to original value
                        wallJumpDirection = WallJumpDirection.FromLeft;     // Set isWallJumping to from left
                        isWallJumping = true;
                        Flip();
                    }
                    else if (Input.GetButtonDown("Jump") && isWallSliding == true && moveInput == 1) // if pushing against wall and jump is pressed
                    {
                        yWallJump = yWallJump / 2;               // Cut Jump height in half
                        wallJumpDirection = WallJumpDirection.FromRight;    // Set isWallJumping to from right
                        isWallJumping = true;
                        Flip();
                    }
                    break;
            }
        } 
    }

    public void DoubleJump()
    {
        if (player.CheckGrounded() == true)
        {
            canDoubleJump = true;       // Enable double jump
        }

        if (Input.GetButtonDown("Jump") && player.CheckGrounded() == false && isfirstJump == false && canDoubleJump == true && isWallJumping == false)  // Check Conditions
        {
            rb.velocity = Vector2.up * doubleJumpForce;     // Perform the double jump
            canDoubleJump = false;                          // Disable double jump
        }
    }

    public void Movement()
    {
        // Set Acceleration
        float acceleration = timeFromZeroToMax * Time.deltaTime;
        float decceleration = timeFromMaxToZero * Time.deltaTime;

        if (activateSpeedBoost == false)
        {
            if (moveInput != 0) { speed = Mathf.MoveTowards(speed, maxSpeed, acceleration); }           // Accelerate if there is move input
            else if (shouldDecelerate) { speed = Mathf.MoveTowards(speed, minSpeed, decceleration); }   // If no move input and should decelerate is true than decelerate
        }
        else
        {
            SpeedBoost(acceleration, decceleration);
        }

        // Flip Character Sprite based on which way we are going
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
        if (speedBoostTimeCounter > 0)
        {
            if (moveInput != 0) { speed = Mathf.MoveTowards(speed, speedBoost, acceleration * 2.5f); }      // Accelerate if there is move input
            else if (shouldDecelerate) { speed = Mathf.MoveTowards(speed, maxSpeed, deceleration / 2.5f); } // If no move input and should decelerate is true than decelerate
            speedBoostTimeCounter -= Time.deltaTime;    // Reduce speed timer
            animator.SetBool("isSpeedBoost", true);     // Set animation to is speed boost
        }
        else if (player.CheckGrounded() == true)
        {
            activateSpeedBoost = false;                 // Disable speed boost when speed timer truns out
            animator.SetBool("isSpeedBoost", false);    // Set animation to normal
        }
    }

    public void SpeedPowerUp(bool activate) // Activate or deactivate speed boost
    {
        if (activate)
        {
            speedBoostTimeCounter = powerupSpeedBoost;
            activateSpeedBoost = true;
        }
        else
        {
            activateSpeedBoost = false;
        }
    }

    public void KillSpeed()
    {
        if (activateSpeedBoost) { speed = maxSpeed; } // Set speed to minimum or maximum based on speed boost
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
