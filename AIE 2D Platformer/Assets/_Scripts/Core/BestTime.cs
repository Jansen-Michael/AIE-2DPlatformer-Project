using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BestTime : MonoBehaviour
{
    public int[] m_Times = new int[1];                // Array to keep track of our scores
    string m_CurrentDirectory;                          // Reference to current directory to store the best time text file in

    //public
    public string Level1ScoreFileName = "level1besttime.txt";   // The name of the best time text file

    void Start()
    {
        // We need to know where we're reading from and writing to.
        // To help us with that, we'll print the current directory to the console.
        m_CurrentDirectory = Application.dataPath;                      // Sets the current directory to the defualt data directory for unity
        Debug.Log("Our current directory is: " + m_CurrentDirectory);   // Prints current directory to the console

        // Load the scores in
        LoadTimesFromFile();
    }


    public void LoadTimesFromFile()
    {
        // Before we try to read a file, we should check that it exists. If it doesn't exist, we'll log a message and abort (so we don't crash the program).
        bool fileExists = File.Exists(m_CurrentDirectory + "\\" + Level1ScoreFileName);

        if (fileExists == true)
        {
            Debug.Log("Found high score file " + Level1ScoreFileName);
        }
        else
        {
            // If file doesn't exist
            Debug.Log("The file " + Level1ScoreFileName + " does not exist. No scores will be loaded.", this);  // Display a error in the console
            return;                                                                                             // Break out of this function
        }

        // Make a new array of default values. This ensures that no old values stick around if we've loaded a scores file in the past.
        m_Times = new int[m_Times.Length];

        // Now we read the file in. We do this using a "StreamReader", which we give our full file path to.
        StreamReader fileReader = new StreamReader(m_CurrentDirectory + "\\" + Level1ScoreFileName);

        // A counter to make sure we don't go past the end of our scores
        //int scoreCount = 0;

        // A if statement to check if the file has an actual value attached
        if (fileReader.Peek() != 0)
        {
            // Read that line into a variable
            string fileLine = fileReader.ReadLine();

            // Try to parse that variable into an int (Beacuase thats where we store our scores as in our game logic)
            int readScore = -1;                                     // Temporary variable to store our read value in for checking
            bool didParse = int.TryParse(fileLine, out readScore);  // See if the line can be converted to an int and store it

            if (didParse)
            {
                // If we successfully read a number, put it in the array.
                m_Times[0] = readScore;
            }
            else
            {
                // If the number couldn't be parsed then there was probably junk in the file.
                Debug.Log("Invalid line in scores file at " + Level1ScoreFileName + ", using default value.", this);     // Print an error
                m_Times[0] = 0;                                                                       // Store a defualt value
            }
            //scoreCount++;   // Increment counter! (So we don't get endless loop)
        }

        fileReader.Close();     // Close the file reader stream!
        Debug.Log("High scores read from " + Level1ScoreFileName);
    }

    public void SaveTimesToFile()
    {
#if UNITY_STANDALONE
        // Create a StreamWriter for our file
        StreamWriter fileWriter = new StreamWriter(m_CurrentDirectory + "\\" + Level1ScoreFileName);

        fileWriter.WriteLine(m_Times[0]);                          // Write the line into the file

        fileWriter.Close();                                         // Close the stream 
        Debug.Log("High scores written to " + Level1ScoreFileName);     // Write a log message.
#endif
    }

    public void AddTime(int newTime)
    {
        // First we find out what index the score it belongs in. This will be the first index with a score lower than the new score.
        int desiredIndex = -1;

        if (m_Times[0] > newTime || m_Times[0] == 0) // If the new score is more than the current score in this index or the index is empty
        {
            desiredIndex = 0;                           // This is the index we want our score to be stored in
        }

        // If no desired index was found then the score isn't high enough to get on the table so we can abort.
        if (desiredIndex < 0)
        {
            Debug.Log("Time of " + newTime + " not low enough for best times list.", this); // Displays message in console
            return;                                                                             // Stops running this function
        }

        // Insert our new score in it's correct place
        m_Times[desiredIndex] = newTime;
        Debug.Log("Time of " + newTime + " entered into best times" + this);
    }
}
