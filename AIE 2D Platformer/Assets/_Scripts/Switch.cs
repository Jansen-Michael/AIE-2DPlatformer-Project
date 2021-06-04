using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    private PlayerController player;
    private CameraHolder cameraHolder; 
    public GameObject lockedDoor;
    public SpriteRenderer keyboardKeyE;
    private bool isDoorLocked = true;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        cameraHolder = FindObjectOfType<CameraHolder>();
        keyboardKeyE.gameObject.SetActive(false);
        isDoorLocked = true;
    }

    private void Update()
    {
        if (keyboardKeyE.gameObject.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(UnlockDoor());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() && isDoorLocked)
        {
            keyboardKeyE.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            keyboardKeyE.gameObject.SetActive(false);
        }
    }

    IEnumerator UnlockDoor()
    {
        Time.timeScale = 0;
        cameraHolder.temporaryTarget = lockedDoor;
        cameraHolder.transform.position = lockedDoor.transform.position;
        player.canMove = false;
        lockedDoor.SetActive(false);
        //Play SFX
        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 1;
        cameraHolder.temporaryTarget = null;
        cameraHolder.transform.position = player.transform.position;
        player.canMove = true;
        keyboardKeyE.gameObject.SetActive(false);
        isDoorLocked = false;
    }
}
