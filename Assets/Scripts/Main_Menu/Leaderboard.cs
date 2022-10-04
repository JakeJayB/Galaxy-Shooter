using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private GameObject content;
    [SerializeField]
    private Text[] scoreText;
    [SerializeField]
    private Text[] dateText;

    private string[] highScoreListChp1 = {"HighScore1", "HighScore2", "HighScore3", "HighScore4", "HighScore5"};
    private string[] highScoreDatesChp1 = {"HighScore1Date", "HighScore2Date", "HighScore3Date", "HighScore4Date", "HighScore5Date"};
    
    private string[] highScoreListChp2 = {"HighScore1Chp2", "HighScore2Chp2", "HighScore3Chp2", "HighScore4Chp2", "HighScore5Chp2"};
    private string[] highScoreDatesChp2 =   {"HighScore1DateChp2", "HighScore2DateChp2", "HighScore3DateChp2", "HighScore4DateChp2", "HighScore5DateChp2"};


    void Start()
    {
        string[] tempValueArr;
        string[] tempDateArr;

        //ensures the panel to start in the default spot
        content.transform.localPosition = new Vector3(0, 0, 0);

        // checking the current scene to determine which arrays to use
        if(SceneManager.GetActiveScene().name == "HighScoreBoardChp1")
        {
            tempValueArr = highScoreListChp1;
            tempDateArr = highScoreDatesChp1;
        }
        else
        {
            tempValueArr = highScoreListChp2;
            tempDateArr = highScoreDatesChp2;
        }

        // 
        for(int i = 0; i < tempValueArr.Length; i++)
        {
            scoreText[i].text = "" + PlayerPrefs.GetInt(tempValueArr[i]);
            dateText[i].text = PlayerPrefs.GetString(tempDateArr[i]);

            
        }
    }


    public void HighScoreMainMenu()   // loads main menu
    {
        SceneManager.LoadScene(0);   // called from a button
    }

}
