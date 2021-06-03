using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    private Button thisButton;
    private SceneLoader SceneManager;
    public string selectedLevel;
    public Image lockedSprite;
    public int level;
    private int currentLevel;

    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        SceneManager = FindObjectOfType<SceneLoader>();

        if (currentLevel >= level)
        {
            lockedSprite.gameObject.SetActive(false);
        }
        else
        {
            lockedSprite.gameObject.SetActive(true);
        }

        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if (currentLevel >= level)
        {
            SceneManager.LoadScene(selectedLevel);
        }
    }
}
