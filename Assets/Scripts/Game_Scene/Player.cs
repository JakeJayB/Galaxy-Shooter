using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] 
    private float _PlayerSpeed = 7.5f;
    [SerializeField]
    private float _SpeedMultiplier = 1.5f;
    [SerializeField]
    private float _fireRate = 0.2f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private GameObject LeftEngine, RightEngine;
    [SerializeField]
    private GameObject _LaserPrefab;
    [SerializeField]
    private GameObject _ExplosionPrefab;
    [SerializeField]
    private GameObject _TripleShotPrefab;

    [SerializeField]
    private bool _IsTripleShotActive = false;
    [SerializeField]
    private bool _IsShieldActive = false;
    private bool _isPeacefulOn;
    private bool _isDifficultyset;
    private bool _isGamePaused;
    private bool _didSequence2Activate;

    [SerializeField]
    private GameObject _ShieldVisualizer;
    [SerializeField]
    private int _score;

    [SerializeField]
    private AudioClip _LaserSound;
    private AudioSource _AudioSource;
    private SpawnManager spawnManager;
    private UIManager uimanager;
    private ScoreManager scoreManager;


    void Start()
    {   
        transform.position = new Vector3(0, -3.46f, 2);

        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        scoreManager =  GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        _AudioSource = GetComponent<AudioSource>();


        if (spawnManager == null)
        {
            Debug.LogError("Spawn manager is NULL");
        }

        if (uimanager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }

        if (_AudioSource == null)
        {
            Debug.LogError("Audio Source is NULL");
        }
        else
        {
            _AudioSource.clip = _LaserSound;
        }

        if(scoreManager == null)
        {
            Debug.LogError("scoreManager is NULL");
        }

        LeftEngine.gameObject.SetActive(false);
        RightEngine.gameObject.SetActive(false);


    }

    void Update()
    {
        if (_isDifficultyset == true)
        {
           CalculateMovement();
           FireLaser();
        }
    }

    void CalculateMovement()
    {
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(HorizontalInput, VerticalInput, 0);

        transform.Translate(direction * _PlayerSpeed * Time.deltaTime);

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 2);
        }
        else if (transform.position.y <= -5)
        {
            transform.position = new Vector3(transform.position.x, -5, 2);
        }


        if (transform.position.x >= 13.35)
        {
            transform.position = new Vector3(-13.35f, transform.position.y, 2);
        }
        else if (transform.position.x <= -13.35)
        {
            transform.position = new Vector3(13.35f, transform.position.y, 2);
        }
    }

    void FireLaser()
    {
        if (Time.timeScale == 1) // if game is not paused; shoot laser
        {

            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            {
                // _fireRate is one lazer per 0.2 seconds
                _canFire = Time.time + _fireRate;

                if (_IsTripleShotActive == true)
                {
                    Instantiate(_TripleShotPrefab, transform.position + new Vector3(0, -0.13f, 0), Quaternion.identity);
                }
                else
                {
                    Instantiate(_LaserPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                }

                _AudioSource.Play();
            }
        }
    }

    public void Damage() // called from Enemy
    {
        // If Shield Powerup is active, don't take damage and return
        if (_IsShieldActive == true)
        {
           _IsShieldActive = false;
           _ShieldVisualizer.SetActive(false);
           return;
        }

        // if Gamemode is not on peaceful, take damage
        if (_isPeacefulOn == false)
        {
            _lives = _lives - 1;
            uimanager.LivesDisplay(_lives);

            // Player Engine Sprite activations
            if (_lives == 2)
            {
                LeftEngine.gameObject.SetActive(true);
            }
            else if (_lives == 1)
            {
                RightEngine.gameObject.SetActive(true);
            }
            //If lives run out, run explosion animation and destroy this object
            else if (_lives == 0)
            {
                Instantiate(_ExplosionPrefab, transform.position, Quaternion.identity);
                _PlayerSpeed = 0;
                scoreManager.evaluateHighScore(_score);
                spawnManager.StopRespawn();
                uimanager.GameOver();
                 
                Destroy(this.gameObject, .25f);
            }
        }
    }

    public void ShieldPowerUp() // called from PowerUp
    {
        _IsShieldActive = true;
        _ShieldVisualizer.SetActive(true);       
    }

    public void TripleShot() // called from PowerUp
    {
        _IsTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
 
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(6);
        _IsTripleShotActive = false;
    }

    public void PowerUpSpeedBoost()
    {
        _PlayerSpeed *= _SpeedMultiplier;
        StartCoroutine(PowerUpSpeedBoostRoutine());
    }

    IEnumerator PowerUpSpeedBoostRoutine()
    {
        // setting the speed directly to a number 
        // to prevent any semantic errors
        yield return new WaitForSeconds(4);
        if (_didSequence2Activate == false)
        {

            _PlayerSpeed = 7.5f;
        }
        if (_didSequence2Activate == true )
        {
            _PlayerSpeed = 9;
        }

    }

    public void Sequence2SpeedBoost() // called from SpawnManager
    {
        // Original player speed: 7.5
        // Sequence 2 player speed: 9
        // both instances have a max speed of: 11.25
        _didSequence2Activate = true;
        _PlayerSpeed *= 1.2f;
        _SpeedMultiplier = 1.25f;
    }

    public void AddToScore(int Score) // called from Enemy
    {
        _score += Score;
        uimanager.ScoreAddon(_score);
        spawnManager.FasterRespawnTime(_score);
    }

    public void PeacefulModeoff() // called from UIManager
    {
        _isPeacefulOn = false;
        _isDifficultyset = true;
    }

    public void peacefulModeOn() // called from UIManager
    {
        _isPeacefulOn = true;
        _isDifficultyset = true;
    }

}

