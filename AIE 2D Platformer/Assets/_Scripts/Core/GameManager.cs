using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Score Variables
    public int currentScore;
    public Text scoreText;

    // Time Variables
    public int startTime = 300;
    private float currentTime;
    public int seconds;
    public Text timeText;

    public int nextLevel;               // the next level unlocked
    public Vector2 lastCheckPointPos;   // player's respawn location

    // References to other instances of the game
    private HighScore highScore;
    private BestTime bestTime;
    private PlayerController player;
    private Boomerang boomerang;
    private GameObject cameraHolder;
    private WinScreen winScreen;
    private GameOverScreen gameOverScreen;

    private bool isPlaying = true;

    void Start()
    {
        // Find and get the components
        highScore = GetComponent<HighScore>();
        bestTime = GetComponent<BestTime>();
        player = FindObjectOfType<PlayerController>();
        boomerang = FindObjectOfType<Boomerang>();
        cameraHolder = GameObject.FindGameObjectWithTag("Camera Holder");
        winScreen = FindObjectOfType<WinScreen>();
        gameOverScreen = FindObjectOfType<GameOverScreen>();

        winScreen.gameObject.SetActive(false);      // Disable win screen
        gameOverScreen.gameObject.SetActive(false); // Disable gameover screen

        // Set up variables
        currentTime = startTime;    // Set timer to start time
        isPlaying = true;           // Set isPlaying to true to allow countdown
    }

    void Update()
    {
        TimeCountDown();
    }

    private void TimeCountDown() // Countdown Time and update timer
    {
        if (isPlaying == false) { return; }

        if (timeText == null)   // if time text does not ecist or hasn't been set print debug and exit
        {
            Debug.LogError("GameManager does not have timer text gameobject linked");
            return; 
        }
        currentTime -= Time.deltaTime;              // Reduce current time
        seconds = Mathf.RoundToInt(currentTime);    // Make current time to a whole number
        timeText.text = "Time: " + seconds;         // update timer text to show proper time

        if (currentTime <= 0) { GameOver(); }
    }

    public void RespawnPlayer()
    {
        boomerang.currentState = Boomerang.State.WithPlayer;
        player.enabled = false;
        player.transform.position = lastCheckPointPos;
        cameraHolder.transform.position = player.transform.position;
        player.GetComponent<PlayerMovement>().SpeedPowerUp(false);
        player.enabled = true;
    }

    public void AddScore(int newScore)  // Add score and update score board
    {
        currentScore += newScore;
        scoreText.text = "Score: " + currentScore;
    }

    /*public void AddTime(float extraTime)
    {
        currentTime += extraTime;
    }*/

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
        gameOverScreen.gameObject.SetActive(true);
    }
} 
