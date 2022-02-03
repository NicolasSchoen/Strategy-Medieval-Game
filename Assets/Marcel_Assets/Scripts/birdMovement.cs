using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdMovement : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime);

        if(transform.position.x > 120)
        {
            transform.position = new Vector3(1.0f, Random.Range(9.0f, 15.0f), Random.Range(1.0f, 99.0f));
        }
        if(transform.position.x < -20)
        {
            transform.position = new Vector3(99.0f, Random.Range(9.0f, 15.0f), Random.Range(1.0f, 99.0f));
        }
        if (transform.position.z > 120)
        {
            transform.position = new Vector3(Random.Range(1.0f, 99.0f), Random.Range(9.0f, 15.0f), 1.0f);
        }
        if (transform.position.z < -20)
        {
            transform.position = new Vector3(Random.Range(1.0f, 99.0f), Random.Range(9.0f, 15.0f), 99.0f);
        }
    }
}
