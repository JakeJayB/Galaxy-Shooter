using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _PowerupSpeed = 3;
    [SerializeField]
    private int _PowerUpID;

    private AudioManager audioManager;
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio Manager").GetComponent<AudioManager>();

        if (audioManager == null)
        {
            Debug.LogError("Audio Manager is NULL");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _PowerupSpeed);

        if (transform.position.y <= -5.8f)
        {
            Destroy(this.gameObject);
        }
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            audioManager.PowerUpAudioClip();
            Destroy(this.gameObject);
            if (player != null)
            { 
                switch (_PowerUpID)
                {
                    default:
                        Debug.Log("Default Value");
                        break;
                    case 0:
                        player.TripleShot();
                        Debug.Log("Collected Triple Shot");
                        break;
                    case 1: 
                        player.PowerUpSpeedBoost();
                        Debug.Log("Collected Speed Boost");
                        break;
                    case 2:
                        player.ShieldPowerUp();
                        Debug.Log("Shield Collected");
                        break;
                }

            }
           
        }
    }

}
