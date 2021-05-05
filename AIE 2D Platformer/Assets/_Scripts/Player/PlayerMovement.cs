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

    private Rigidbody2D rb;
    private Animator animator;
    private bool facingRight = true;

    public bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    // Jump
    public float jumpForce = 20f;
    public bool doubleJump = true;
    public float doubleJumpForce = 40f;
    private bool canDoubleJump = false;
    private float jumpTimeCounter;
    public float jumpTime;
    private bool isfirstJump;

    public GameObject spriteBody;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        speedBoostTimeCounter = speedBoostTime;
    }

    private void Update()
    {
        //Jump(doubleJump);
    }

    public void Jump(bool doubleJumpEnabled)
    {
        if (doubleJumpEnabled == true && isGrounded == true)
        {
            canDoubleJump = true;
            jumpTimeCounter = jumpTime;
        }
        else if (isGrounded == true)
        {
            jumpTimeCounter = jumpTime;
        }

        if (Input.GetButtonDown("Jump") && isGrounded == true)
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

        if (Input.GetButtonDown("Jump") && isGrounded == false && canDoubleJump == true && isfirstJump == false)
        {
            rb.velocity = Vector2.up * doubleJumpForce;
            canDoubleJump = false;
        }
    }

    void FixedUpdate()
    {
        //Movement();
    }

    public void Movement()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        // Set Acceleration
        float acceleration = timeFromZeroToMax * Time.fixedDeltaTime;
        float decceleration = timeFromMaxToZero * Time.fixedDeltaTime;

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

        speed = Mathf.Clamp(speed, minSpeed, speedBoost);
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);


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
            speedBoostTimeCounter -= Time.fixedDeltaTime;
            animator.SetBool("isSpeedBoost", true);
        }
        else if (isGrounded == true)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = spriteBody.transform.localScale;
        scaler.x *= -1;
        spriteBody.transform.localScale = scaler;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, checkRadius);
    }
}
