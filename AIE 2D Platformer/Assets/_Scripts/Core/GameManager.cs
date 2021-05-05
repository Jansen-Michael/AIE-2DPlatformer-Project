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
    private GameObject cameraHolder;
    private WinScreen winScreen;

    private bool isPlaying = true;

    private void Awake()
    {
        /*if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        } else
        {
            Destroy(gameObject);
        }*/
    }

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
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

    private void TimeCountDown(bool countdown)
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
        player.enabled = false;
        player.transform.position = lastCheckPointPos;
        cameraHolder.transform.position = player.transform.position;
        player.enabled = true;
    }

    public void AddScore(int newScore)
    {
        currentScore += newScore;
        scoreText.text = "Score: " + currentScore;
    }

    public void AddTime(float extraTime)
    {
        currentTime += extraTime;
    }

    public void LevelComplete()
    {
        winScreen.gameObject.SetActive(true);
        winScreen.SetPointsScore(currentScore);
        winScreen.SetTimeScore(startTime, seconds);
        player.canMove = false;
        isPlaying = false;
        player.GetComponent<PlayerMovement>().KillSpeed();
    }
} 
