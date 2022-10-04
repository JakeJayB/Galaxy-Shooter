 using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _ScoreText;
    private int _Score = 0;
    [SerializeField]
    private Image _LiveImg;
    [SerializeField]
    private Sprite[] _LivesSprites;
    [SerializeField]
    private Text _GameOverText;
    [SerializeField]
    private Text _RestartText;
    [SerializeField]
    private Text _MainMenuText;
    [SerializeField]
    private Text _PlayerSpeedUpText;
    [SerializeField]
    private Text _PowerUpSpawnTime;
    [SerializeField]
    private Text _StartGameText;
    [SerializeField]
    private Text _ChooseDifficultyText;
    [SerializeField]
    private Text _PeacefulNoteText;
    [SerializeField]
    private Text _PauseMenuText;
    [SerializeField]
    private Text _WannaQuit;
    [SerializeField]
    private Text _ChapterUnlockedText;


    [SerializeField]
    private Button _NormalDifficultyButton;
    [SerializeField]
    private Button _PeacefulDifficultyButton;
    [SerializeField]
    private Button _UnpauseGameButton;
    [SerializeField]
    private Button _MainMenuButton;
    [SerializeField]
    private Button _RestartButton;
    [SerializeField]
    private Button _YesButton;
    [SerializeField]
    private Button _noButton;

    private GameManager gameManager;
    private Player player;
    private Asteroid asteroid;
    private SpawnManager spawnManager;

    [SerializeField]
    private Text[] _Sequences;
    [SerializeField]
    private Text[] _EnemySpawnTimeDecreased;

    private bool _isSequence2Starting;
    private bool _isSequence3Starting;
    private bool _isDifficultySet;
    private bool _isGameOver;
    private bool _isGamePaused;
    private bool _isPlayerQuitting;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<Player>();
        asteroid = GameObject.Find("Asteroid").GetComponent<Asteroid>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }
        if (player == null)
        {
            Debug.LogError("Player is NULL");
        }
        if (asteroid == null)
        {
            Debug.LogError("Asteroid is NULL");
        }
        if (spawnManager == null)
        {
            Debug.LogError("SpawnManager is NULL");
        }
 
        _ScoreText.text = "Score: " + _Score;

        _GameOverText.gameObject.SetActive(false);
        _RestartText.gameObject.SetActive(false);
        _MainMenuText.gameObject.SetActive(false);
        _Sequences[0].gameObject.SetActive(false);
        _Sequences[1].gameObject.SetActive(false);
        _Sequences[2].gameObject.SetActive(false);
        _EnemySpawnTimeDecreased[0].gameObject.SetActive(false);
        _EnemySpawnTimeDecreased[1].gameObject.SetActive(false);
        _PlayerSpeedUpText.gameObject.SetActive(false);
        _PowerUpSpawnTime.gameObject.SetActive(false);
        _StartGameText.gameObject.SetActive(false);
        _PauseMenuText.gameObject.SetActive(false);
        _UnpauseGameButton.gameObject.SetActive(false);
        _MainMenuButton.gameObject.SetActive(false);
        _RestartButton.gameObject.SetActive(false);
        _YesButton.gameObject.SetActive(false);
        _noButton.gameObject.SetActive(false);
        _WannaQuit.gameObject.SetActive(false);

        _NormalDifficultyButton.gameObject.SetActive(true);
        _PeacefulDifficultyButton.gameObject.SetActive(true);
        _PeacefulNoteText.gameObject.SetActive(true);
        _ChooseDifficultyText.gameObject.SetActive(true);
    }

    private void Update()
    {
        PauseGame();
        InGameQuitApplication();
    }

    public void StartSequence1()
    {
        _StartGameText.gameObject.SetActive(false);
        StartCoroutine(Sequence1TextRoutine());
    }

    public void ScoreAddon(int CurrentScore) // called from Player
    {
        _ScoreText.text = "Score: " + CurrentScore; 
    }

    public void LivesDisplay(int CurrentLives) // called from player
    {
        _LiveImg.sprite = _LivesSprites[CurrentLives];
    }

    public void TutorialButton()
    {
        gameManager.TutorialInGameButton();
    }

    public void GameOver() // called from Player
    {
        StartCoroutine(GamerOverFlickerRoutine());
        _RestartText.gameObject.SetActive(true);
        _MainMenuText.gameObject.SetActive(true);
        gameManager.GameOver();
        _isGameOver = true;
    }

    private IEnumerator GamerOverFlickerRoutine()  
    {
        while (true)
        {
            _GameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(.5f);
            _GameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(.5f);
        }
    }

    private IEnumerator Sequence1TextRoutine()
    {
        _Sequences[0].gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        _Sequences[0].gameObject.SetActive(false);
    }

    public void StartSequence2() // called from SpawnManager
    {
        _isSequence2Starting = true;
        StartCoroutine(Sequence2and3TextRoutine());
    }

    public void StartSequence3() // called from Spawnmanager
    {
        _isSequence3Starting = true;
        StartCoroutine(Sequence2and3TextRoutine());
    }

    private IEnumerator Sequence2and3TextRoutine()
    {

        if (_isSequence2Starting == true)
        {
            _isSequence2Starting = false;
            _Sequences[1].gameObject.SetActive(true);
            _EnemySpawnTimeDecreased[0].gameObject.SetActive(true);
            _PlayerSpeedUpText.gameObject.SetActive(true);
            yield return new WaitForSeconds(6);
            _Sequences[1].gameObject.SetActive(false);
            _EnemySpawnTimeDecreased[0].gameObject.SetActive(false);
            _PlayerSpeedUpText.gameObject.SetActive(false);
        }
        else if (_isSequence3Starting == true)
        {
            _isSequence3Starting = false;
            _Sequences[2].gameObject.SetActive(true);
            _EnemySpawnTimeDecreased[1].gameObject.SetActive(true);
            _PowerUpSpawnTime.gameObject.SetActive(true);
            yield return new WaitForSeconds(6);
            _Sequences[2].gameObject.SetActive(false);
            _EnemySpawnTimeDecreased[1].gameObject.SetActive(false);
            _PowerUpSpawnTime.gameObject.SetActive(false);

            if(_ChapterUnlockedText != null)
            {
                _ChapterUnlockedText.gameObject.SetActive(true);
                yield return new WaitForSeconds(6);
                _ChapterUnlockedText.gameObject.SetActive(false);
            }

        }
    }

    public void NoPlayerInvincibility() // called from a button
    {
        player.PeacefulModeoff();
        asteroid.DifficultyisSet();
        spawnManager.isPeacefulOffVoid();

        _NormalDifficultyButton.gameObject.SetActive(false);
        _PeacefulDifficultyButton.gameObject.SetActive(false);
        _PeacefulNoteText.gameObject.SetActive(false);
        _ChooseDifficultyText.gameObject.SetActive(false);
        _StartGameText.gameObject.SetActive(true);
        _isDifficultySet = true;
    }

    public void YesPlayerInvincibility() // called from a button
    {
        player.peacefulModeOn();
        asteroid.DifficultyisSet();

        _NormalDifficultyButton.gameObject.SetActive(false);
        _PeacefulDifficultyButton.gameObject.SetActive(false);
        _PeacefulNoteText.gameObject.SetActive(false);
        _ChooseDifficultyText.gameObject.SetActive(false);
        _StartGameText.gameObject.SetActive(true);
        _isDifficultySet = true;
    }

    private void PauseGame() // called from Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && _isDifficultySet == true && _isGameOver == false && _isPlayerQuitting == false)
        {
            _PauseMenuText.gameObject.SetActive(true);
            _UnpauseGameButton.gameObject.SetActive(true);
            _MainMenuButton.gameObject.SetActive(true);
            _RestartButton.gameObject.SetActive(true);
            _isGamePaused = true;

            Time.timeScale = 0;
        }
    }

    public void ResumeGame() // Called from a button
    {
        if (_isDifficultySet == true)
        {
            _PauseMenuText.gameObject.SetActive(false);
            _UnpauseGameButton.gameObject.SetActive(false);
            _MainMenuButton.gameObject.SetActive(false);
            _RestartButton.gameObject.SetActive(false);
            _isGamePaused = false;

            Time.timeScale = 1;
        }
    }    
    private void InGameQuitApplication() // called from Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _isGameOver == false && _isDifficultySet == true  && _isGamePaused == false)
        {
            _YesButton.gameObject.SetActive(true);
            _noButton.gameObject.SetActive(true);
            _WannaQuit.gameObject.SetActive(true);
            _isPlayerQuitting = true;

            Time.timeScale = 0;
        }
    }

    public void CancelQuitRequest() // called from a button
    {
        _YesButton.gameObject.SetActive(false);
        _noButton.gameObject.SetActive(false);
        _WannaQuit.gameObject.SetActive(false);
        _isPlayerQuitting = false;

        Time.timeScale = 1;

    }
}


