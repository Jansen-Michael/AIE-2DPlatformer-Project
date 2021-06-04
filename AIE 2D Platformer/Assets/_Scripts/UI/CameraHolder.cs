using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public float dampTime = 0f;         // Value for a smoothing effect

    public PlayerController playerTarget;      // Reference to our player
    public GameObject temporaryTarget;

    private Vector3 moveVelocity;
    private Vector3 desiredPosition;

    private void Awake()
    {
        playerTarget = FindObjectOfType<PlayerController>();

        if (playerTarget != true)       // Checks if there is a target set
        {
            Debug.LogError(gameObject.name + " does not have a target set!");   // Gives a error message to console
        }
    }

    private void FixedUpdate()
    {
        if (temporaryTarget != null)
        {
            desiredPosition = temporaryTarget.transform.position;                                                        // Set where we want the object to go
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, dampTime); // Update the object's position
        } 
        else
        {
            desiredPosition = playerTarget.transform.position;                                                        // Set where we want the object to go
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, dampTime); // Update the object's position
        }
    }
}