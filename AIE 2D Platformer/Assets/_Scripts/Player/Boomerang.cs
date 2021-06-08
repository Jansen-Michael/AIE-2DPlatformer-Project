using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    private PlayerController player;        // Reference to player
    private Rigidbody2D rb;                 // Reference to rigidbody2D
    private TrailRenderer trailRenderer;    // Reference to Trail Renderer
    private SpriteRenderer spriteRender;    // Reference to Sprite Renderer
    private CircleCollider2D circleCollider;// Reference to Circle Collider
    private BoxCollider2D boxCollider;      // Reference to Box Collider
    private Animator animator;              // Reference to the animator
    public PhysicsMaterial2D boomerangPhysics; // Reference to the boomerang physics material

    private float throwForce = 35f;         // Throw force value
    public float minThrowForce = 25f;       // Minimum thorw force used when charge value is 1
    public float maxThrowForce = 45f;       // Maxmimum throw force used when charge value is 3

    public float recallStartSpeed = 40f;    // Recall starting speed
    public float recallMaxSpeed = 100f;     // Recall max speed
    public float recallAccelation = 25f;    // Acceleration for recalls speed value
    private float recallSpeed;              // Recall current speed value
    public float minSpeedBeforeStop = 15f;  // Speed before boomerang prepares to recall
    public float timeBeforeRecall = 1.25f;  // The Time before the boomerang return to the player after stopping
    public float grabDistance = 2f;         // Distance between player and boomerang before grabbed

    private int chargeValue;                // The charge value of the throw
    private bool isPrepareForRecall = false;// Bool to make sure PrepareForRecall is only called once at a time
    private bool wasFrozen = false;         // Bool to check if boomerang was just frozen
    
    public enum State           // All the different states the boomerang has
    {
        WithPlayer,
        Thrown,
        Frozen,
        Recalling
    }
    public State currentState; // Boomerang's current state

    private void Awake() // Priorty Reference
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        circleCollider = GetComponent<CircleCollider2D>();
        Physics2D.IgnoreCollision(circleCollider, player.GetComponent<BoxCollider2D>());
        currentState = State.WithPlayer;
    }

    private void Start() // Get Component for all references
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        trailRenderer = GetComponent<TrailRenderer>();
        spriteRender = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        trailRenderer.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case State.Thrown:  // What to do check for during thrown state
                float throwSpeed = rb.velocity.magnitude;
                if (throwSpeed < minSpeedBeforeStop)    // check if the boomerang is getting close to a stop
                {
                    if (isPrepareForRecall == false) { StartCoroutine(PrepareForRecall()); } // Start coroutine for recall
                }
                player.GetComponent<Animator>().SetBool("isThrowing", false);
                break;

            case State.Recalling:   // What to check for during recall state
                Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;  // The player's direction
                recallSpeed = Mathf.MoveTowards(recallSpeed, recallMaxSpeed, recallAccelation);         // Increase the value of 
                rb.isKinematic = true;
                rb.velocity = dirToPlayer * recallSpeed;

                if (Vector2.Distance(transform.position, player.transform.position) < grabDistance) // if player can grab the boomerang
                {
                    currentState = State.WithPlayer;
                    trailRenderer.enabled = false;
                    rb.velocity = Vector2.zero;
                    wasFrozen = false;                  // Reset the wasFrozen to allow freeze on next throw
                }
                break;
        }
    }

    private void LateUpdate()
    {
        switch(currentState)
        {
            case State.WithPlayer:  // Set all interactable and visual boomerang values to false
                circleCollider.enabled = false;
                boxCollider.enabled = false;
                spriteRender.enabled = false;
                trailRenderer.enabled = false;
                animator.SetBool("isFrozen", false);
                break;
            default:
                circleCollider.enabled = true;
                spriteRender.enabled = true;
                break;
        }
    }

    public void ThrowBoomerang(Vector3 throwDir, float chargePercentage)
    {
        player.GetComponent<Animator>().SetBool("isThrowing", true);
        transform.position = player.transform.position;             // Start at player location
        rb.isKinematic = false;                                     // Make object not kinematic
        rb.sharedMaterial = boomerangPhysics;                       // Set physics material back to it's defualt

        DetermindChargeValue(chargePercentage);
        switch(chargeValue)
        {
            case 1:
                throwForce = minThrowForce;                         // Set throw force to the minimum value
                break;
            case 2:
                throwForce = ((maxThrowForce - minThrowForce) / 2) + minThrowForce; // Set throw force to a value between the min and max
                break;
            case 3:
                throwForce = maxThrowForce;                         // Set throw force to the maximum value
                break;
        }
        rb.AddForce(throwDir * throwForce, ForceMode2D.Impulse);    // Add force to object in direction of mouse
        trailRenderer.enabled = true;                               // Enable trail render
        currentState = State.Thrown;                                // Set current state to thrown
    }

    private void DetermindChargeValue(float chargePercentage)       // Function to determind the charge level based on player mouse input
    {
        if (chargePercentage < 0.4)
        {
            chargeValue = 1;    // Low Charge
        }
        else if (chargePercentage < 0.85f)
        {
            chargeValue = 2;    // Medium Charge
        }
        else
        {
            chargeValue = 3;    // Large Charge
        }
    }

    public void FreezeBoomerang()
    {
        if (!isFrozen() && !wasFrozen)      // Not currently frozen and wasfrozen is false
        {
            currentState = State.Frozen;    // Set current state to frozen
            rb.velocity = Vector2.zero;     // Stop boomerang
            rb.isKinematic = true;          // Set boomerang to kinematic
            rb.sharedMaterial = null;       // Set physics material to null
            boxCollider.enabled = true;     // Enable box collider
            trailRenderer.enabled = false;  // Disable trail renderer
            animator.SetBool("isFrozen", true); // Set animation to is frozen
            gameObject.layer = LayerMask.NameToLayer("Platform");   // Set Layer to Platform
        } 
        else if (isFrozen() && !wasFrozen)  // is currently frozen
        {
            rb.sharedMaterial = boomerangPhysics;               // Set physics material back to it's defualt
            boxCollider.enabled = false;                        // Disable box collider
            trailRenderer.enabled = true;                       // Enable trail renderer
            animator.SetBool("isFrozen", false);                // Set animation back to defualt
            wasFrozen = true;                                   // was frozen is set to true so we know we were just frozen
            gameObject.layer = LayerMask.NameToLayer("Default");// Set layer to Defualt
            recallSpeed = recallStartSpeed;                     // Set recall speed back to the defualt value
            currentState = State.Recalling;                     // Set current state to recall
        }
    }

    public bool isWithPlayer()
    {
        return currentState == State.WithPlayer;    // Return true if currentState is withplayer
    }

    public bool isFrozen()
    {
        return currentState == State.Frozen;        // Returns true if current state is frozen
    }

    IEnumerator PrepareForRecall()  // Stop the boomerang before making it recall
    {
        isPrepareForRecall = true;          // Set bool to true to prevent this corutine to be called multiple times
        rb.velocity = Vector2.zero;         // Stop the boomerang
        recallSpeed = recallStartSpeed;     // Resets the recall speed
        yield return new WaitForSeconds(timeBeforeRecall);
        isPrepareForRecall = false;         // Set bool to false to allow this corutine to be run again
        if (currentState == State.Thrown)   // Check to make sure we are in the thrown state and not freeze
        {
            currentState = State.Recalling; // Set boomerang to recalling
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Coin>()) { collider.GetComponent<Coin>().Collected(); }   // If it is a coin then collect it
    }
}