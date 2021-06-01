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

    public void SetPointsScore(int finalScore)
    {
        FinalScoreText.text = "Total Score: " + finalScore;      // Set text to display total score
    }

    public void SetTimeScore(float finalTime)
    {
        FinalTimeText.text = "Total Time: " + finalTime + "s";  // Set text to display total time
    }

    public void SetHighScore(int highScore)
    {
        BestScoreText.text = "Best Score:" + highScore;         // Set text to display the high score
    }

    public void SetBestTime(float bestTime)
    {
        BestTimeText.text = "Best Time: " + bestTime + "s";     // Set text to display the best time
    }
}