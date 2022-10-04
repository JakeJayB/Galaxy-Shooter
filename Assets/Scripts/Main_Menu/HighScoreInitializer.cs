using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreInitializer : MonoBehaviour
{
    // These two Lists are Keys. Each Key has a unique value
    private string[] highScoreListChp1 = {"HighScore1", "HighScore2", "HighScore3", "HighScore4", "HighScore5"};
    private string[] highScoreDatesChp1 = {"HighScore1Date", "HighScore2Date", "HighScore3Date", "HighScore4Date", "HighScore5Date"};
    
    private string[] highScoreListChp2 = {"HighScore1Chp2", "HighScore2Chp2", "HighScore3Chp2", "HighScore4Chp2", "HighScore5Chp2"};
    private string[] highScoreDatesChp2 =   {"HighScore1DateChp2", "HighScore2DateChp2", "HighScore3DateChp2", "HighScore4DateChp2", "HighScore5DateChp2"};

    
    void Start()
    {
        bool areScoresInitalized = PlayerPrefs.GetInt("areHighScoresInitalized") == 1 ? true : false;

        if(!areScoresInitalized)
        {
            Debug.Log("Scores are getting Initialized");
            for(int i = 0; i < highScoreListChp1.Length; i++)
            {
                PlayerPrefs.SetInt(highScoreListChp1[i], 0);
                PlayerPrefs.SetInt(highScoreListChp2[i], 0);

                PlayerPrefs.SetString(highScoreDatesChp1[i], "Date N/A");
                PlayerPrefs.SetString(highScoreDatesChp2[i], "Date N/A");

            }
            PlayerPrefs.SetInt("areHighScoresInitalized", 1);
            return;
        }
        Debug.Log("Score weren't Initialized");
    }


}
