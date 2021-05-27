using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    // Reference to the winscreen text objects
    public Text FinalScoreText;
    public Text BestScoreText;
    public Text FinalTimeText;
    public Text BestTimeText;

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current Scene
        Time.timeScale = 1f;
    }

    public void LevelSelect(string sceneName)
    {
        SceneManager.LoadScene(sceneName);   // Load Scene
    }

    public void NextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;    // Get current scene build index
        SceneManager.LoadScene(currentScene + 1);                       // Load the next scene on the build list
    }

    public void SetPointsScore(float score, float startTime, float timeLeft)
    {
        float finalScore = score + startTime - timeLeft;    // Calculate final score
        FinalScoreText.text = "Total Score: " + score;      // Set text to display total score
    }

    public void SetTimeScore(float startTime, float timeLeft)
    {
        float finalTime = startTime - timeLeft;             // Calculate how much time it took to complete level
        FinalTimeText.text = "Total Time: " + finalTime;    // Set text to display total time
    }
}