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
        // starting player position
        transform.position = new Vector3(0, -3.46f, 2);

        // finding these gameobjects in game scene and accessing their scripts
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        scoreManager =  GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        _AudioSource = GetComponent<AudioSource>();


        // Null Checking 
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

    // this method is called 60 time a second to support game simulation
    void Update()
    {
        if (_isDifficultyset == true)
        {
           CalculateMovement();
           FireLaser();
        }
    }

    /*** 
        translates the player on screen using WASD keys. Also limits the player to a certain bound
    */
    void CalculateMovement()
    {
        float HorizontalInput = Input.GetAxis("Horizontal"); // gets accecss to keys A and D
        float VerticalInput = Input.GetAxis("Vertical"); // gets access to keys W and S

        Vector3 direction = new Vector3(HorizontalInput, VerticalInput, 0);

        // Translate = Move
        // moves based on the direction controlled by WASD keys and player speed
        transform.Translate(direction * _PlayerSpeed * Time.deltaTime);

        // limiting player y-axis to -5 and 0 
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 2);
        }
        else if (transform.position.y <= -5)
        {
            transform.position = new Vector3(transform.position.x, -5, 2);
        }

        // if player reaches the far right/left end of screen, teleport to the other side
        if (transform.position.x >= 13.35)
        {
            transform.position = new Vector3(-13.35f, transform.position.y, 2);
        }
        else if (transform.position.x <= -13.35)
        {
            transform.position = new Vector3(13.35f, transform.position.y, 2);
        }
    }

    /*** 
        translates the player on screen using WASD keys. Also limits the player to a certain bound
    */
    void FireLaser()
    {
        // if game is not paused; shoot laser
        if (Time.timeScale == 1) 
        {

            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            {
                // _fireRate is one lazer per 0.2 seconds
                _canFire = Time.time + _fireRate;

                // if Triple Shot is activated, spawn in the Triple Shot prefab
                if (_IsTripleShotActive == true)
                {
                    Instantiate(_TripleShotPrefab, transform.position + new Vector3(0, -0.13f, 0), Quaternion.identity);
                }
                else // else, just shoot a normal laser
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

    public void ShieldPowerUp() // called from PowerUp class
    {
        _IsShieldActive = true;
        _ShieldVisualizer.SetActive(true);       
    }

    public void TripleShot() // called from PowerUp class
    {
        _IsTripleShotActive = true;

        // StartCoroutine() is simply calling another method which will helps us create a timer
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    /*** 
        Activates Triple Shot power up for 6 seconds
    */
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

    /*** 
        Activates Speed Boost power up for 4 seconds
    */
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

    // There are 3 sequences in a level. In sequence 2, the player gets a speed increase (not a speed boost power up)
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
        
        // Updates UI score as player kills enemy
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

