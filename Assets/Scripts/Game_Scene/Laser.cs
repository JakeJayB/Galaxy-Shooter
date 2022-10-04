using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField]
    private float _LaserSpeed = 11f;    

    void Update()
    {
       transform.Translate(Vector3.up * _LaserSpeed * Time.deltaTime);


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
