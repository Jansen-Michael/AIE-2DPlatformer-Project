using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMove : MonoBehaviour
{
    private Rigidbody2D rb;                 // Reference to Rigidbody
    private Animator animator;              // Reference to our Animator
    public float dashSpeed;                 // Dash speed value
    private float dashTime;                 // The countdown timer of the dash
    public float startDashTime;             // The time of how long the dash will last for
    public ParticleSystem dashParticle;     // Reference to the dash particle
    public bool canDash = true;             // Bool to check if we can dash or not

    public float dashChargeTime;                // The time to charge up a dash
    public float currentDashChargeTimer = 0f;   // The current dash charge timer
    private bool startDashCharge;               // Bool used to check whether we should start charging the dash or not

    public enum DashDirection { None, Up, Right, Down, Left};           // Enumerator type used to determine the dash direction
    private DashDirection dashDirection;                                // This is where we store the actual dash's direfction

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();           // Get the Rigidbody Component
        animator = GetComponent<Animator>();        // Get the Animator Component
        dashTime = startDashTime;                   // Set the dash time
        dashDirection = DashDirection.None;         // Set dash direction
    }

    void Update()
    {
        if (GetComponent<PlayerController>().CheckGrounded())   // Check if player touch the ground
        {
            startDashCharge = true; // Start the dash charge process
        }

        if (startDashCharge == true)
        {
            if (currentDashChargeTimer >= dashChargeTime)
            {
                canDash = true;     // Allow player to dash if dash charge is full
            }
            else
            {
                currentDashChargeTimer += Time.deltaTime;   // Add charge based on time
                Mathf.Clamp(currentDashChargeTimer, 0, dashChargeTime);
            }
        }
    }

    public void Dash()
    {
        if (dashDirection == DashDirection.None && canDash)    // If we are currently not dashing than check where to dash
        {
            if (Input.GetAxisRaw("Horizontal") == 1 && Input.GetKeyDown(KeyCode.LeftShift))         // Check if player dash to the right
            {
                dashDirection = DashDirection.Right;    // Set dash direction
                Instantiate(dashParticle, transform.position, Quaternion.identity); // Spawn the dash effect
            }
            else if (Input.GetAxisRaw("Horizontal") == -1 && Input.GetKeyDown(KeyCode.LeftShift))   // Check if player dash to the right
            {
                dashDirection = DashDirection.Left;     // Set dash direction
                Instantiate(dashParticle, transform.position, Quaternion.identity); // Spawn the dash effect
            }
            else if (Input.GetAxisRaw("Vertical") == 1 && Input.GetKeyDown(KeyCode.LeftShift))      // Check if player dash to the right
            {
                dashDirection = DashDirection.Up;       // Set dash direction
                Instantiate(dashParticle, transform.position, Quaternion.identity); // Spawn the dash effect
            }
            else if (Input.GetAxisRaw("Vertical") == -1 && Input.GetKeyDown(KeyCode.LeftShift))     // Check if player dash to the right
            {
                dashDirection = DashDirection.Down;     // Set dash direction
                Instantiate(dashParticle, transform.position, Quaternion.identity); // Spawn the dash effect
            }
        }
        else if (canDash == true)
        {
            if (dashTime <= 0) // If dash time is done execute
            {
                StopDash(); // Stop dashing
            }
            else
            {
                if (dashDirection != DashDirection.Down || GetComponent<PlayerController>().CheckGrounded()) 
                { 
                    dashTime -= Time.deltaTime; // Decrease time if not dashing down and if it is dashing down then don't decrease time untill checkground is true
                    if (dashDirection == DashDirection.Down) { dashTime = 0; }  // If is dashing down and is on ground than end timer
                }
                

                switch (dashDirection)  // Perform a dash based on the dash direction
                {
                    case DashDirection.Right:
                        rb.gravityScale = 0;
                        rb.velocity = Vector2.zero;
                        transform.Translate(Vector2.right * dashSpeed * Time.deltaTime);// Dashes player
                        animator.SetBool("isDashing", true);                            // Set animation value isDashing to true
                        break;
                    case DashDirection.Left:
                        rb.gravityScale = 0;
                        rb.velocity = Vector2.zero;
                        transform.Translate(Vector2.left * dashSpeed * Time.deltaTime); // Dashes player
                        animator.SetBool("isDashing", true);                            // Set animation value isDashing to true
                        break;
                    case DashDirection.Up:
                        rb.gravityScale = 0;
                        rb.velocity = Vector2.zero;
                        transform.Translate(Vector2.up * dashSpeed * Time.deltaTime);   // Dashes player
                        animator.SetBool("isDashing", true);                            // Set animation value isDashing to true
                        break;
                    case DashDirection.Down:
                        rb.gravityScale = 0;
                        rb.velocity = Vector2.zero;
                        rb.velocity = Vector2.down * 200;       // Slam the player to the bottom
                        animator.SetBool("isDashing", true);    // Set animation value isDashing to true
                        break;
                }
            }
        }
    }

    public void StopDash()
    {
        GetComponent<PlayerMovement>().KillSpeed();     // Kills all player speed/momentem
        animator.SetBool("isDashing", false);           // Set animation value isDashing to false
        dashDirection = DashDirection.None;             // Set dash to none
        dashTime = startDashTime;                       // Reset the dash time
        canDash = false;                                // Disable dash
        currentDashChargeTimer = 0f;                    // Reset the dash charge timer
        startDashCharge = false;                        // Stop the dash charge process
    }
}
