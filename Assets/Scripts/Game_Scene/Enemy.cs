using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4f;
    private Player player;
    private Animator _Animator;
    private AudioManager audioManager;

    [SerializeField]
    private GameObject _EnemyLaserPrefab;

    private bool _isEnemyDead;
    
    void Start()
    {
        Instantiate(_EnemyLaserPrefab, transform.position + new Vector3(0, -1.312f, 0), Quaternion.identity);

        player = GameObject.Find("Player").GetComponent<Player>();
        audioManager = GameObject.FindGameObjectWithTag("Audio Manager").GetComponent<AudioManager>();

        if (player == null)
        {
            Debug.LogError("Player is NULL");
        }

        _Animator = gameObject.GetComponent<Animator>();

        if (_Animator == null)
        {
            Debug.LogError("Animator is NULL");
        }

        if (audioManager == null)
        {
            Debug.LogError("Audio Manager is NULL");
        }

        if (SceneManager.GetActiveScene().name == "Chapter 2") // Fastened Enemy Speed in Chapter 2
        {
            _enemySpeed = 5f;
        }

    }


    void Update()
    {
        EnemyMovement();

    }

    void EnemyMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y <= -6.52)
        {
            float RandomX = Random.Range(-11f, 11f);
            transform.position = new Vector3(RandomX, 8.5f, 2);

            if (_isEnemyDead == false)
            {
                Instantiate(_EnemyLaserPrefab, transform.position + new Vector3(0, -1.312f, 0), Quaternion.identity);  //instantiating another laser
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {

            _Animator.SetTrigger("OnEnemyDeath");
            audioManager.ExplosionAudioClip();
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<BoxCollider2D>());
            _isEnemyDead = true;
            _enemySpeed = 1;
            Destroy(this.gameObject, 2.6f);

            if (player != null)
            {
                player.Damage();
            }

        }

        if (other.tag == "Laser")
        {
            if (player != null)
            {
                player.AddToScore(10);
            }

            _Animator.SetTrigger("OnEnemyDeath");
            audioManager.ExplosionAudioClip();
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<BoxCollider2D>());
            _isEnemyDead = true;
            _enemySpeed = 1;
            Destroy(this.gameObject, 2.6f);

            if (other != null)
            {
                Destroy(other.gameObject);
            }
        }
    }

 //  public void AllowEnemyToPause()
 //  {
 //      _isEnemyInGame = true;
 //  }

 //  public void GamePaused()
 //  {
 //      if (_isEnemyInGame == true && Input.GetKeyDown(KeyCode.P))
 //      {
 //          _allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

 //          foreach (GameObject Enemies in _allEnemies)
 //          { 
 //              _enemySpeed = 0;
 //          }
 //      }
 //  }
}
