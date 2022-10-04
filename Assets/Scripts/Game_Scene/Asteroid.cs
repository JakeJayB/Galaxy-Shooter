using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private int _asteroidRotationSpeed = 50;
    [SerializeField]
    private GameObject _ExplosionPrefab;

    private Player player;
    private UIManager uiManager;
    private SpawnManager spawnManager;
    private AudioManager audioManager;

    private bool _isDifficultySet;
    

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        uiManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
        spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
        audioManager = GameObject.FindGameObjectWithTag("Audio Manager").GetComponent<AudioManager>();

        if (player == null)
        {
            Debug.LogError("Player is NULL");
        }

        if (uiManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }


        if (spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }

        if (audioManager == null)
        {
            Debug.LogError("Audio Manager is NULL");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (_isDifficultySet == true)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * _asteroidRotationSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_ExplosionPrefab, transform.position, Quaternion.identity);
            audioManager.ExplosionAudioClip();
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<CircleCollider2D>());
            Destroy(this.gameObject, .5f);
            Destroy(other.gameObject);
            uiManager.StartSequence1();
            spawnManager.StartSpawning();
        }
    }
    

    public void DifficultyisSet()
    {
        _isDifficultySet = true;
    }
}
