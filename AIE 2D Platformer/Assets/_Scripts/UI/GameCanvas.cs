using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    // Screens
    public WinScreen winScreen;
    public GameObject gameOverScreen;
    public GameObject pauseScreen;

    // UI Text
    public Text scoreText;
    public Text timeText;

    // Player
    PlayerController player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    public void PauseGame()
    {
        if (pauseScreen.activeSelf == true)
        {
            Time.timeScale = 1;             // Unpause Game
            player.canMove = true;          // Allow player movement and input
            pauseScreen.SetActive(false);   // Disable pause menu
        }
        else
        {
            Time.timeScale = 0;             // Pause Game
            player.canMove = false;         // Disable player movement and input
            pauseScreen.SetActive(true);    // Enable pause menu
        }
    }
}
