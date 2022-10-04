using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver = false;
    private bool _isThereNewHighScore = false;

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(Application.loadedLevel); // Restarted current Chapter Scene
            
        }

        if (Input.GetKeyDown(KeyCode.M) && _isGameOver == true)
        {
            if(_isThereNewHighScore == true && SceneManager.GetActiveScene().name == "Chapter 1")
            {
                SceneManager.LoadScene(7);  // Chp1 leaderboard scene

            }
            else if(_isThereNewHighScore == true && SceneManager.GetActiveScene().name == "Chapter 2")
            {
                SceneManager.LoadScene(9); // Chp2 leaderboard scene
            }
            else
            {
                SceneManager.LoadScene(0);  // Main Menu 
            }
        }
    }

    public void TutorialInGameButton()  // called from button
    {
        SceneManager.LoadScene(2); // Loads to tutorial 1
    }

    public void MainMenuButton()    // called from button
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void RestartButton() // called from button
    {
        SceneManager.LoadScene(Application.loadedLevel);
        Time.timeScale = 1;
    }

    public void QuitGame() // called from button
    {
        Application.Quit();
    }

    public void GameOver() //called from GameManager
    {
        _isGameOver = true;
    }

    public void NewHighScore() // called from ScoreManager
    {
        _isThereNewHighScore = true;
    }


}
