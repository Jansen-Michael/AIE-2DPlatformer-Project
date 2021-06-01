using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int currentScore;
    public Text scoreText;

    public int startTime = 300;
    private float currentTime;
    public int seconds;
    public Text timeText;

    private static GameManager instance;
    public Vector2 lastCheckPointPos;

    private HighScore highScore;
    private BestTime bestTime;
    private PlayerController player;
    private Boomerang boomerang;
    private GameObject cameraHolder;
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
        winScreen = FindObjectOfType<WinScreen>();
        winScreen.gameObject.SetActive(false);

        currentTime = startTime;
        isPlaying = true;
    }

    void Update()
    {
        TimeCountDown(isPlaying);
    }

    private void TimeCountDown(bool countdown) // Countdown Time and update timer
    {
        if (countdown == false) { return; }

        if (timeText == null) 
        {
            Debug.LogError("GameManager does not have timer text gameobject linked");
            return; 
        }
        currentTime -= Time.deltaTime;
        seconds = Mathf.RoundToInt(currentTime);
        timeText.text = "Time: " + seconds;
    }

    public void RespawnPlayer()
    {
        boomerang.currentState = Boomerang.State.WithPlayer;
        player.enabled = false;
        player.transform.position = lastCheckPointPos;
        cameraHolder.transform.position = player.transform.position;
        player.GetComponent<PlayerMovement>().activateSpeedBoost = false;
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
        winScreen.gameObject.SetActive(true);
        int finalScore = currentScore + seconds;
        int finalTime = startTime - seconds;
        highScore.AddScore(finalScore);
        highScore.SaveScoresToFile();
        bestTime.AddTime(finalTime);
        bestTime.SaveTimesToFile();

        winScreen.SetPointsScore(finalScore);
        winScreen.SetTimeScore(finalTime);
        winScreen.SetHighScore(highScore.m_Scores[0]);
        winScreen.SetBestTime(bestTime.m_Times[0]);

        player.canMove = false;
        isPlaying = false;
        player.GetComponent<PlayerMovement>().KillSpeed();
    }
} 
