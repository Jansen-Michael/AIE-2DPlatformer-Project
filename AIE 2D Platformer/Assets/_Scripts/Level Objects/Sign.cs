using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{
    private PlayerController player;
    public GameObject popUpBox;
    public Text textBox;
    public SpriteRenderer keyboardKeyE;
    public string signText;
    private bool isReading;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        keyboardKeyE.gameObject.SetActive(false);
    }

    void Update()
    {
        if (keyboardKeyE.gameObject.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PopUpText();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>()) // If player enters range of the sign
        {
            keyboardKeyE.gameObject.SetActive(true);    // Activate E keyboard key sprite
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>()) // If player exits range of the sign
        {
            keyboardKeyE.gameObject.SetActive(false);   // Deactivate E keyboard key sprite
        }
    }

    private void PopUpText()
    {
        if (isReading == false)
        {
            Time.timeScale = 0;         // Stop time to temporary pause the game
            popUpBox.SetActive(true);   // Enable the popup box
            textBox.text = signText;    // Set the text to the sign text
            player.canMove = false;     // Disable player input and movement
            isReading = true;           // Set is reading to true
        }
        else
        {
            Time.timeScale = 1;         // Stop time to temporary pause the game
            popUpBox.SetActive(false);  // Enable the popup box
            player.canMove = true;     // Enable player input and movement
            isReading = false;          // Set is reading to true
        }
    }
}
