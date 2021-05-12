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
        player = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        speedBoostTimeCounter = speedBoostTime;
        wallJumpDirection = WallJumpDirection.None;
    }

    private void Update()
    {
        /*if (player.CheckRightWall() && player.CheckGrounded() == false && moveInput != 0)
        {
            isWallSliding = true;
        }
        else if (player.CheckLeftWall() && player.CheckGrounded() == false && moveInput != 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }

        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        if (Input.GetButtonDown("Jump") && isWallSliding == true && moveInput == -1)
        {
            wallJumpDirection = WallJumpDirection.FromLeft;
            isWallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }
        else if (Input.GetButtonDown("Jump") && isWallSliding == true && moveInput == 1)
        {
            wallJumpDirection = WallJumpDirection.FromRight;
            isWallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }*/
    }

    private void FixedUpdate()
    {
        // Movement
        speed = Mathf.Clamp(speed, minSpeed, speedBoost);
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        //Wall Jump
        if (isWallSliding)
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

        /*if (Input.GetButtonDown("Jump") && isWallSliding == true && moveInput == -1)
        {
            wallJumpDirection = WallJumpDirection.FromLeft;
            isWallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }
        else if (Input.GetButtonDown("Jump") && isWallSliding == true && moveInput == 1)
        {
            wallJumpDirection = WallJumpDirection.FromRight;
            isWallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }*/

        if (wallJumpTimer <= 0) { isWallJumping = false; wallJumpTimer = wallJumpTime; }
        else 
        {
            switch (wallJumpDirection)
            {
                case WallJumpDirection.None:    // Allow both left and right wall jump
                    if (Input.GetButtonDown("Jump") && isWallSliding == true && moveInput == -1) // if pushing against wall and jump is pressed
                    {
                        print("From left");
                        wallJumpDirection = WallJumpDirection.FromLeft;     // Set isWallJumping to from left
                        isWallJumping = true;
                        Flip();
                    }
                    else if (Input.GetButtonDown("Jump") && isWallSliding == true && moveInput == 1) // if pushing against wall and jump is pressed
                    {
                        print("From right");
                        rb.velocity = new Vector2(-xWallJumpForce * 2, xWallJumpForce);
                        wallJumpDirection = WallJumpDirection.FromRight;    // Set isWallJumping to from right
                        isWallJumping = true;
                        Flip();
                    }
                    break;

                case WallJumpDirection.FromLeft:    // Allow only right wall jump
                    if (Input.GetButtonDown("Jump") && isWallSliding == true && moveInput == 1) // if pushing against wall and jump is pressed
                    {
                        print("From right22");
                        wallJumpDirection = WallJumpDirection.FromRight;    // Set isWallJumping to from right
                        isWallJumping = true;
                        Flip();
                    }
                    break;

                case WallJumpDirection.FromRight:   // Allow only left wall jump
                    if (Input.GetButtonDown("Jump") && isWallSliding == true && moveInput == -1) // if pushing against wall and jump is pressed
                    {
                        print("From left22");
                        wallJumpDirection = WallJumpDirection.FromLeft;     // Set isWallJumping to from left
                        isWallJumping = true;
                        Flip();
                    }
                    break;
            }
        }
    }

    public void DoubleJump(bool jumpInput)
    {
        if (player.CheckGrounded() == true)
        {
            canDoubleJump = true;       // Enable double jump
        }

        if (jumpInput && player.CheckGrounded() == false && isfirstJump == false && canDoubleJump == true &&wallJumpDirection == WallJumpDirection.None)
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

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = spriteBody.transform.localScale;
        scaler.x *= -1;
        spriteBody.transform.localScale = scaler;
    }
}
