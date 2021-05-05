using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float dampTime = 0f;         // Value for a smoothing effect

    public Transform playerTarget;      // Reference to our player

    private Vector3 moveVelocity;
    private Vector3 desiredPosition;

    private void Awake()
    {
        if (playerTarget != true)       // Checks if there is a target set
        {
            Debug.LogError(gameObject.name + " does not have a target set!");   // Gives a error message to console
        }
    }

    private void FixedUpdate()
    {
        desiredPosition = playerTarget.position;                                                                  // Set where we want the object to go
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, dampTime); // Update the object's position
    }
}