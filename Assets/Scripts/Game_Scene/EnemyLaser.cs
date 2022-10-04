using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _EnemyLaserSpeed = 10f;
    private Player player;

    // Update is called once per frame
    void Update()
    {

        transform.Translate(UnityEngine.Vector3.down * _EnemyLaserSpeed * Time.deltaTime);

        if (transform.position.y <= -6.23f)
        { 
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            if (player == null)
            {
                Debug.LogError("Player is NULL");
            }
            Destroy(this.gameObject);
            player.Damage();
        }
    }
}
