using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main_Menu : MonoBehaviour
{
    [SerializeField]
    private Text _Creatorsname;
    [SerializeField]
    private GameObject _ResetPanel;
    [SerializeField]
    private GameObject _ConfirmationPanel;
    [SerializeField]
    private Button _Chapter2Button;

    private int _levelID;

    private void Start()
    {
        LevelUnlock();

        if(_ResetPanel != null)
        {
            _ResetPanel.gameObject.SetActive(false);
            _ConfirmationPanel.gameObject.SetActive(false);
        }

        if (SceneManager.GetActiveScene().name == "Main_Menu")
        {
            StartCoroutine(CreatorsNameFlickerRoutine());
            _Creatorsname.gameObject.SetActive(true);
        }
    }

    IEnumerator CreatorsNameFlickerRoutine()
    {
        while (true)
        {
            _Creatorsname.gameObject.SetActive(false);
            yield return new WaitForSeconds(1);
            _Creatorsname.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
        }
    }

    public void LevelUnlock()
    {
        // if scene = pre game
        if (SceneManager.GetActiveScene().name == "Pre-Game")
        {
            _levelID = PlayerPrefs.GetInt("Level");

            switch (_levelID)
            {
                default:
                    Debug.Log("Default Value");
                    break;
                case 1:
                    _Chapter2Button.interactable = true;
                    break;
            }
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(6);  // Chapter 1 Game
    }

    public void LoadChapter2()
    {
        SceneManager.LoadScene(8);  // Chapter 2

    }

    public void PreGame()
    {
        SceneManager.LoadScene(5); // Pre Game
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene(2);  // Tutorial #1
    }

    public void Tutorial1()
    {
        SceneManager.LoadScene(3); ;  // Tutorial #2
    }

    public void Tutorial2()
    {
        SceneManager.LoadScene(4);  // Enjoy The Game Screen
    }

    public void Tutorial3()
    {
        SceneManager.LoadScene(0);  // Load back Main Menu
    }

    public void VersionDetails()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetGameRequest()
    {
        _ResetPanel.gameObject.SetActive(true);
    }

    public void CancelResetRequest()
    {
        _ResetPanel.gameObject.SetActive(false);
    }

    public void FullyResetGame()
    {
        PlayerPrefs.DeleteAll();
        _ResetPanel.gameObject.SetActive(false);
        _ConfirmationPanel.gameObject.SetActive(true);
    }
}
