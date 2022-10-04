using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    // These two Lists are Keys. Each Key has a unique value
    private string[] highScoreListChp1 = {"HighScore1", "HighScore2", "HighScore3", "HighScore4", "HighScore5"};
    private string[] highScoreDatesChp1 = {"HighScore1Date", "HighScore2Date", "HighScore3Date", "HighScore4Date", "HighScore5Date"};

    private string[] highScoreListChp2 = {"HighScore1Chp2", "HighScore2Chp2", "HighScore3Chp2", "HighScore4Chp2", "HighScore5Chp2"};
    private string[] highScoreDatesChp2 =   {"HighScore1DateChp2", "HighScore2DateChp2", "HighScore3DateChp2", "HighScore4DateChp2", "HighScore5DateChp2"};
    
    /*
    private void debugLoop()
    {
                
                
        if(SceneManager.GetActiveScene().name == "Chapter 1")
        {
            for(int i = 0; i < highScoreListChp1.Length; i++)
            {
                Debug.Log(PlayerPrefs.GetInt(highScoreListChp1[i]));
                Debug.Log(PlayerPrefs.GetString(highScoreDatesChp1[i]));
            } 
        }
        else
        {
            for(int i = 0; i < highScoreListChp2.Length; i++)
            {
                Debug.Log(PlayerPrefs.GetInt(highScoreListChp2[i]));
                Debug.Log(PlayerPrefs.GetString(highScoreDatesChp2[i]));
            }       
        }


    }
    */

    // Ensures that the scores are sorted and the new score is saved
    private void repositionHighScores(int score, int indexToStart, string[] arrValues, string[] arrDates)
    {
        // saves the new score initially 
        int replaceValuePlaceHolder = score;
        string replaceDatePlaceHolder = DateTime.Now.ToString("M/d/yyyy");
        int nextValue = 0;
        string nextDate = "";

        
        for(int i = indexToStart; i < arrValues.Length; i++)
        {
            if(i != arrValues.Length-1)
            {
                nextValue = PlayerPrefs.GetInt(arrValues[i]);
                nextDate = PlayerPrefs.GetString(arrDates[i]);
            }
            PlayerPrefs.SetInt(arrValues[i], replaceValuePlaceHolder);
            PlayerPrefs.SetString(arrDates[i], replaceDatePlaceHolder);

            replaceValuePlaceHolder = nextValue;
            replaceDatePlaceHolder = nextDate;          
        }
    }


    public void evaluateHighScore(int previousScore) // Called from Player 
    {
        string[] tempValueArr;
        string[] tempDateArr;

        if(SceneManager.GetActiveScene().name == "Chapter 1")
        {
            tempValueArr = highScoreListChp1;
            tempDateArr = highScoreDatesChp1;
        }
        else
        {
            tempValueArr = highScoreListChp2;
            tempDateArr = highScoreDatesChp2;
        }

        // Goes through each element of the list to determine if 
        // new score is a high score
        for(int i = 0; i < tempValueArr.Length; i++)
        {
            // if a high score key is still at its default, new score is a high score
            if(PlayerPrefs.GetInt(tempValueArr[i]) == 0 && PlayerPrefs.GetString(tempDateArr[i]) == "Date N/A")
            {
                PlayerPrefs.SetInt(tempValueArr[i], previousScore);
                PlayerPrefs.SetString(tempDateArr[i], DateTime.Now.ToString("M/d/yyyy"));
                gameManager.NewHighScore();
                break;
            }
            // if a high score key has a defined value and is lower
            // than new score, new score is a high score and reposition highscores
            else if(previousScore >= PlayerPrefs.GetInt(tempValueArr[i]))
            {
                repositionHighScores(previousScore, i, tempValueArr, tempDateArr);
                gameManager.NewHighScore();
                break;
            }
        }
       //debugLoop();
    }
}