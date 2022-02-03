using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerController : MonoBehaviour
{
    public float height = 4f;
    private Vector3 oldPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void interactWithObject(GameObject other)
    {
        if(other.transform.position.y > transform.position.y + 1)
        {
            other.transform.position = oldPos;
        }
        else
        {
            oldPos = other.transform.position;
            other.transform.position = transform.position + new Vector3(0, height, 0);
        }
    }
}
