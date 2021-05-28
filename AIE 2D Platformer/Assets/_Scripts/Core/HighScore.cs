using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    public int[] m_Scores = new int[10];                // Array to keep track of our scores
    string m_CurrentDirectory;                          // Reference to current directory to store the high scores text file in

    public string m_ScoreFileName = "highscores.txt";   // The name of the high score text file

    void Start()
    {
        // We need to know where we're reading from and writing to.
        // To help us with that, we'll print the current directory to the console.
        m_CurrentDirectory = Application.dataPath;                      // Sets the current directory to the defualt data directory for unity
        Debug.Log("Our current directory is: " + m_CurrentDirectory);   // Prints current directory to the console

        // Load the scores in
        LoadScoresFromFile();
    }


    public void LoadScoresFromFile()
    {
#if UNITY_STANDALONE
        // Before we try to read a file, we should check that it exists. If it doesn't exist, we'll log a message and abort (so we don't crash the program).
        bool fileExists = File.Exists(m_CurrentDirectory + "\\" + m_ScoreFileName);

        if (fileExists == true)
        {
            Debug.Log("Found high score file " + m_ScoreFileName);
        }
        else
        {
            // If file doesn't exist
            Debug.Log("The file " + m_ScoreFileName + " does not exist. No scores will be loaded.", this);  // Display a error in the console
            return;                                                                                         // Break out of this function
        }

        // Make a new array of default values. This ensures that no old values stick around if we've loaded a scores file in the past.
        m_Scores = new int[m_Scores.Length];

        // Now we read the file in. We do this using a "StreamReader", which we give our full file path to.
        StreamReader fileReader = new StreamReader(m_CurrentDirectory + "\\" + m_ScoreFileName);

        // A counter to make sure we don't go past the end of our scores
        int scoreCount = 0;

        // A while loop, which runs as long as there is data to be read (peek) and we haven't reached the end of our scores array.
        while (fileReader.Peek() != 0 && scoreCount < m_Scores.Length)
        {
            // Read that line into a variable
            string fileLine = fileReader.ReadLine();

            // Try to parse that variable into an int (Beacuase thats where we store our scores as in our game logic)
            int readScore = -1;                                     // Temporary variable to store our read value in for checking
            bool didParse = int.TryParse(fileLine, out readScore);  // See if the line can be converted to an int and store it

            if (didParse)
            {
                // If we successfully read a number, put it in the array.
                m_Scores[scoreCount] = readScore;
            }
            else
            {
                // If the number couldn't be parsed then there was probably junk in the file.
                Debug.Log("Invalid line in scores file at " + scoreCount + ", using default value.", this);     // Print an error
                m_Scores[scoreCount] = 0;                                                                       // Store a defualt value
            }
            scoreCount++;   // Increment counter! (So we don't get endless loop)
        }

        fileReader.Close();     // Close the file reader stream!
        Debug.Log("High scores read from " + m_ScoreFileName);
#endif
    }

    public void SaveScoresToFile()
    {
#if UNITY_STANDALONE
        // Create a StreamWriter for our file
        StreamWriter fileWriter = new StreamWriter(m_CurrentDirectory + "\\" + m_ScoreFileName);

        for (int i = 0; i < m_Scores.Length; i++)
        {
            fileWriter.WriteLine(m_Scores[i]);                      // Loop through our score array and write each line to the file
        }

        fileWriter.Close();                                         // Close the stream 
        Debug.Log("High scores written to " + m_ScoreFileName);     // Write a log message.
#endif
    }

    public void AddScore(int newScore)
    {
        // First we find out what index the score it belongs in. This will be the first index with a score lower than the new score.
        int desiredIndex = -1;

        for (int i = 0; i < m_Scores.Length; i++)           // Loop throuf=gh all our scores
        {
            if (m_Scores[i] > newScore || m_Scores[i] == 0) // If the new score is less than the current score in this index or the index is empty
            {
                desiredIndex = i;                           // This is the index we want our score to be stored in
                break;                                      // stops the loop when we've found the correct index to put our score in
            }
        }

        // If no desired index was found then the score isn't high enough to get on the table so we can abort.
        if (desiredIndex < 0)
        {
            Debug.Log("Score of " + newScore + " not high enough for high scores list.", this); // Displays message in console
            return;                                                                             // Stops running this function
        }

        // If scores is not good enough, we move all of the scores after the desired index back by one position
        for (int i = m_Scores.Length - 1; i > desiredIndex; i--)    // Loop backwards through our array
        {
            m_Scores[i] = m_Scores[i - 1];                          // Move the current score back one Index
        }

        // Insert our new score in it's correct place
        m_Scores[desiredIndex] = newScore;
        Debug.Log("Score of " + newScore + " entered into high scores at position " + desiredIndex, this);
    }
}
