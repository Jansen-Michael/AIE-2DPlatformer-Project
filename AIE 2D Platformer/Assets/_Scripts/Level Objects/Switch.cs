using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    private PlayerController player;        // Reference to the player
    private CameraHolder cameraHolder;      // Reference to the camera
    public GameObject lockedDoor;           // Reference to the locked door
    public SpriteRenderer keyboardKeyE;     // Reference to the E keyboard key sprite
    private bool isDoorLocked = true;       // Bool to check is door is locked

    void Start()
    {
        player = FindObjectOfType<PlayerController>();      // Find player reference
        cameraHolder = FindObjectOfType<CameraHolder>();    // Find camera refernce
        keyboardKeyE.gameObject.SetActive(false);           // Disable the E keyboard key sprite
        isDoorLocked = true;                                // Set door is locked to true
    }

    private void Update()
    {
        if (keyboardKeyE.gameObject.activeSelf == true)     // Check if E keyboard key sprite is active if it is than player is on range
        {
            if (Input.GetKeyDown(KeyCode.E))                // Check if the E key is pressed by the player
            {
                StartCoroutine(UnlockDoor());               // Start the UnlockDoor Coroutine
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() && isDoorLocked) // If player enters range of switch and door is lock
        {
            keyboardKeyE.gameObject.SetActive(true);    // Enable the E keyboard key sprite
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>()) // If player leaves the range of the switch disable the E keyboard key sprite
        {
            keyboardKeyE.gameObject.SetActive(false);
        }
    }

    IEnumerator UnlockDoor()
    {
        Time.timeScale = 0;     // Stop time to temporary pause the game
        cameraHolder.temporaryTarget = lockedDoor;  // Set camera target to the locked door
        cameraHolder.transform.position = lockedDoor.transform.position;    // Move camera to the door position
        player.canMove = false;         // Disable player input and movement
        lockedDoor.SetActive(false);    // Disable locked door
        isDoorLocked = false;           // Set is door locked to false
        //Play SFX
        yield return new WaitForSecondsRealtime(1.8f);    // Wait for 1.8 real time seconds before running next section of code

        Time.timeScale = 1;     // Resume time to allow the game to move again
        cameraHolder.temporaryTarget = null;    // Set camera temporary target to null so it defualts back to the player
        cameraHolder.transform.position = player.transform.position;    // Move camera to the player position
        player.canMove = true;      // Allow player movement and input
        keyboardKeyE.gameObject.SetActive(false);   // Disable the E keyboard key sprite
    }
}
