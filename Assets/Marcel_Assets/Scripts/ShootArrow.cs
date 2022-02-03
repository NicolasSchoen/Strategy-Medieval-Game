using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootArrow : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float shootForce = 20f;
    public AudioSource arrowShootSound;
    private Camera archerCam;
    private Transform arrowSpawn;

    // Start is called before the first frame update
    void Start()
    {
        archerCam = transform.root.GetComponentInChildren<Camera>();
        arrowSpawn = archerCam.transform.GetChild(0).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            archerCam.fieldOfView = 30f;
        }
        if (Input.GetMouseButtonUp(1))
        {
            archerCam.fieldOfView = 60f;
        }
    }

    void Shoot()
    {        
        GameObject newArrow = Instantiate(arrowPrefab, arrowSpawn.position, Quaternion.identity) as GameObject;
        Rigidbody rb = newArrow.GetComponent<Rigidbody>();
        rb.velocity = arrowSpawn.forward * shootForce;
    }

    void playSound()
    {
        arrowShootSound.Play();
    }
}
