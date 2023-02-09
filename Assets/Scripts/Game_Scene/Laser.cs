using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField]
    private float _LaserSpeed = 11f;    

    void Update()
    {
        // laser always will shoot up
       transform.Translate(Vector3.up * _LaserSpeed * Time.deltaTime);

        // if y-axis of lazer reaches beyond 8, destory it
        if (transform.position.y >= 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

}
