using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catapultController : MonoBehaviour
{
    public GameObject bullet;
    public bool isEnemy = true;
    public float shootFrequency = 2f;
    private float deltaTime = 0f;
    private GameObject nearestEnemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //check distance to player, if close, shoot
        if (Input.GetKeyDown(KeyCode.F))
        {
            shootAtPlayer();
        }
        if (isEnemy && nearestEnemy != null && deltaTime <= 0f)
        {
            shootAtPlayer();
        }
        deltaTime -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "person" || other.tag == "military")
        {
            nearestEnemy = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (nearestEnemy != null && (other.tag == "person" || other.tag == "military"))
        {
            nearestEnemy = null;
        }
    }

    void shootAtPlayer()
    {
        GameObject currentBullet = Instantiate(bullet,transform.position + new Vector3(0,1f,0), transform.rotation);
        //currentBullet.GetComponent<Rigidbody>().AddForce(((nearestEnemy.transform.position - currentBullet.transform.position)+ currentBullet.transform.up)*5, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(((nearestEnemy.transform.position - currentBullet.transform.position)) * 4, ForceMode.Impulse);
        currentBullet.transform.parent = null;
        deltaTime = shootFrequency;
    }

}
