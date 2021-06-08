using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Score Variables
    private int currentScore;
    private Text scoreText;

    // Time Variables
    public int startTime = 300;
    private float currentTime;
    public int seconds;
    private Text timeText;

    public int nextLevel;               // the next level unlocked
    public Vector2 lastCheckPointPos;   // player's respawn location

    // References to other instances of the game
    private HighScore highScore;
    private BestTime bestTime;
    private PlayerController player;
    private Boomerang boomerang;
    private GameObject cameraHolder;
    private GameCanvas gameCanvas;
    private WinScreen winScreen;

    private bool isPlaying = true;

    void Start()
    {
        // Find and get the components
        highScore = GetComponent<HighScore>();
        bestTime = GetComponent<BestTime>();
        player = FindObjectOfType<PlayerController>();
        boomerang = FindObjectOfType<Boomerang>();
        cameraHolder = GameObject.FindGameObjectWithTag("Camera Holder");
        gameCanvas = FindObjectOfType<GameCanvas>();
        winScreen = gameCanvas.winScreen;

        // Set up Text
        scoreText = gameCanvas.scoreText;
        timeText = gameCanvas.timeText;

        // Set up variables
        currentTime = startTime;    // Set timer to start time
        isPlaying = true;           // Set isPlaying to true to allow countdown

        if (currentTime <= 0) { isPlaying = false; }    // if we start with no time than don't start playing
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { gameCanvas.PauseGame(); }   // Pause Game
        TimeCountDown();    // Start CountDown
    }

    private void TimeCountDown() // Countdown Time and update timer
    {
        if (isPlaying == false) { return; }

        currentTime -= Time.deltaTime;              // Reduce current time
        seconds = Mathf.RoundToInt(currentTime);    // Make current time to a whole number
        timeText.text = "Time: " + seconds;         // update timer text to show proper time

        if (currentTime <= 0) { GameOver(); }
    }

    public void RespawnPlayer() // Respawn the Player
    {
        boomerang.currentState = Boomerang.State.WithPlayer;            // Return Boomerang to player
        player.enabled = false;                                         // Disable Player
        player.transform.position = lastCheckPointPos;                  // Move player to last check point
        cameraHolder.transform.position = player.transform.position;    // Move camera to player
        player.GetComponent<PlayerMovement>().SpeedPowerUp(false);      // Disable Speed Boost
        player.GetComponent<DashMove>().StopDash();                     // Stop any dash movements
        player.enabled = true;                                          // Enable the player
    }

    public void AddScore(int newScore)  // Add score and update score board
    {
        currentScore += newScore;
        scoreText.text = "Score: " + currentScore;
    }

    public void AddTime(float extraTime)    // Add more time to the timer
    {
        currentTime += extraTime;
    }

    public void LevelComplete()
    {
        // Calculate and save final score and time
        int finalScore = currentScore + seconds;
        int finalTime = startTime - seconds;
        highScore.AddScore(finalScore);
        highScore.SaveScoresToFile();
        bestTime.AddTime(finalTime);
        bestTime.SaveTimesToFile();

        // Activate win screen and show resualts
        winScreen.gameObject.SetActive(true);
        winScreen.SetPointsScore(finalScore);
        winScreen.SetTimeScore(finalTime);
        winScreen.SetHighScore(highScore.m_Scores[0]);
        winScreen.SetBestTime(bestTime.m_Times[0]);

        // Disable timer and player control
        player.canMove = false;
        isPlaying = false;
        player.GetComponent<PlayerMovement>().KillSpeed();

        if (nextLevel > PlayerPrefs.GetInt("CurrentLevel"))
        {
            PlayerPrefs.SetInt("CurrentLevel", nextLevel);
        }
    }

    public void GameOver()
    {
        // Disable player control and show gameover screen
        player.canMove = false;
        isPlaying = false;
        player.GetComponent<PlayerMovement>().KillSpeed();
        player.gameObject.SetActive(false);
        boomerang.gameObject.SetActive(false);
        gameCanvas.gameOverScreen.gameObject.SetActive(true);
    }
} 
