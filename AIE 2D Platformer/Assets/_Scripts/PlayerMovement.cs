using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private float moveInput;

    private Rigidbody2D rb;
    private bool facingRight = true;

    public bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    public float jumpForce = 20f;
    public bool doubleJump = true;
    public float doubleJumpForce = 40f;
    private bool canDoubleJump = false;
    public float jumpTimeCounter;
    public float jumpTime;
    private bool isfirstJump;

    public GameObject spriteBody;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (doubleJump == true && isGrounded == true)
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
            } else {
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
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxisRaw("Horizontal");
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
