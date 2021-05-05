using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    public Text FinalScoreText;
    public Text BestScoreText;
    public Text FinalTimeText;
    public Text BestTimeText;

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void LevelSelect()
    {

    }

    public void NextLevel()
    {

    }

    public void SetPointsScore(float score)
    {
        FinalScoreText.text = "Total Score: " + score;
    }

    public void SetTimeScore(float startTime, float timeLeft)
    {
        float finalTime = startTime - timeLeft;
        FinalTimeText.text = "Total Time: " + finalTime;
    }
}