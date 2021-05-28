using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int currentScore;
    public Text scoreText;

    public float startTime = 300f;
    private float currentTime;
    private int seconds;
    public Text timeText;

    private static GameManager instance;
    public Vector2 lastCheckPointPos;

    private PlayerController player;
    private Boomerang boomerang;
    private GameObject cameraHolder;
    private WinScreen winScreen;

    private bool isPlaying = true;

    void Start()
    {
        // Find and get the components
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
        int seconds = Mathf.RoundToInt(currentTime);
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
        winScreen.SetPointsScore(currentScore, startTime, seconds);
        winScreen.SetTimeScore(startTime, seconds);
        player.canMove = false;
        isPlaying = false;
        player.GetComponent<PlayerMovement>().KillSpeed();
    }


} 
