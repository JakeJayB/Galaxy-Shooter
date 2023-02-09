using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private float _enemySpawnTime;

    [SerializeField]
    private float _PowerUpSpawnTime;
    private int RandomPowerUp;
    [SerializeField]
    private int _endPowerUpRange;

    [SerializeField]
    private bool stopRespawn = false;
    private bool _hasThisBeenCalledOnce;
    private bool isPeacefulOff = false;

    [SerializeField]
    public GameObject _EnemyPrefab;
    [SerializeField]
    private GameObject _EnemyContainer;
    [SerializeField]
    private GameObject[] PowerUps;
    private GameObject[] Enemies;
    private Animator _enemyAnimator;
    private UIManager uimanager;
    private Player player;


    // at the very start of the game scene
    void Start()
    {
        uimanager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (uimanager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }

        if (player == null)
        {
            Debug.LogError("Player is NULL");
        }

    }

    /*** 
        When the player shoots the asteroid, this function is called. 
        Starts the Enemy and PowerUp spawn routine
    */
    public void StartSpawning() // Called from Asteroid
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }


    /*** 
        consistently spawns enemies into game scene as long as 
        the game has started and the player is alive 
    */
    IEnumerator SpawnEnemyRoutine()
    { 
        // waits for 6 seconds before proceeding
        yield return new WaitForSeconds(6);

        // sets powerup spawn time based on current level
        // checks current level with SceneManager.GetActiveScene().name
        if(SceneManager.GetActiveScene().name == "Chapter 1") // chapter 1
        {
            _enemySpawnTime = 1.45f;
        }
        else // chapter 2
        {
            _enemySpawnTime = 1.0f;
        }

        while (stopRespawn == false)
        {
            // gets a random x-axis value in between -11 and 10 x-coordinate, and spawns enemy at that x value
            Vector3 direction = new Vector3(Random.Range(-11f, 10f), 8.5f, 2);
            GameObject newEnemy = Instantiate(_EnemyPrefab, direction, Quaternion.identity);
            newEnemy.transform.parent = _EnemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnTime);
        }
    }

    /*** 
        consistently spawns powerups into game scene as long as 
        the game has started and the player is alive 
    */
    IEnumerator SpawnPowerUpRoutine()
    {
        // waits for 6 seconds before proceeding
        yield return new WaitForSeconds(6);

        // sets powerup spawn time based on current level
        // checks current level with SceneManager.GetActiveScene().name
        if (SceneManager.GetActiveScene().name == "Chapter 1") // Chapter 1
        {

            _PowerUpSpawnTime = Random.Range(5, 8);
            _endPowerUpRange = 3;
            while (stopRespawn == false)
            {
                Vector3 direction = new Vector3(Random.Range(-9.06f, 9.06f), 8.5f, 2);
                RandomPowerUp = Random.Range(0, _endPowerUpRange);
                Instantiate(PowerUps[RandomPowerUp], direction, Quaternion.identity);
                yield return new WaitForSeconds(_PowerUpSpawnTime);
            }
        }
        else // Chapter 2
        {
            _PowerUpSpawnTime = 4;
            _endPowerUpRange = 3;
            while (stopRespawn == false)
            {
                Vector3 direction = new Vector3(Random.Range(-9.06f, 9.06f), 8.5f, 2);
                RandomPowerUp = Random.Range(0, _endPowerUpRange);
                Instantiate(PowerUps[RandomPowerUp], direction, Quaternion.identity);
                yield return new WaitForSeconds(_PowerUpSpawnTime);
            }
        }
    }

    /*** 
        This function is called when player is dead.
        Stops respawn of enemies and powerups
    */
    public void StopRespawn() // Called from Player
    {
        stopRespawn = true;
        // gets all existing enemies in game scene 
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // killing all existing enemies
        foreach (GameObject Enemy in Enemies) 
        {
            _enemyAnimator = Enemy.GetComponent<Animator>();
            _enemyAnimator.SetTrigger("OnEnemyDeath");
            Destroy(Enemy, 2.6f);
        }
    }


    /*** 
        fastens Respawn time of Powerups and Enemies based on current chapter and score
        @param:
            CurrentScore - Player's current score value
    */
    public void FasterRespawnTime(int CurrentScore) // Called from Player
    {
        if (SceneManager.GetActiveScene().name == "Chapter 1") // Chapter 1 Sequences
        {
            // fastens enemy spawntime
            if (CurrentScore == 250)
            {
                uimanager.StartSequence2();
                player.Sequence2SpeedBoost();
                _enemySpawnTime = .8f; 

            }
            // fastens Enemy and Powerup spawntimes
            else if (CurrentScore == 800)
            {
                uimanager.StartSequence3();
                _enemySpawnTime = .5f;
                _PowerUpSpawnTime = Random.Range(3, 5);

                if (isPeacefulOff == true && PlayerPrefs.GetInt("Level") == 0)
                {
                    PlayerPrefs.SetInt("Level", 1);
                }
            }
        }
        else // Chapter 2 Sequences
        {
            // fastens enemy spawntime
            if (CurrentScore == 300)
            {
                uimanager.StartSequence2();
                player.Sequence2SpeedBoost();
                _enemySpawnTime = .70f;

            }
            // fastens Enemy and Powerup spawntimes
            // also gives more range to Powerup Array [0-4]
            else if (CurrentScore == 1200)
            {
                uimanager.StartSequence3();
                _enemySpawnTime = .5f;
                _PowerUpSpawnTime = 2.5f;
                _endPowerUpRange = 4;
            }
        }

    }


    public void isPeacefulOffVoid() // Called from UIManager
    {
        isPeacefulOff = true;
    }
}  


