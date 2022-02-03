using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    public GameObject magicPrefab;
    public AudioSource magicCastSound;
    private Transform magicSpawn;
    public float shootForce = 20f;

    float nextAttackTime = 0f;

    personController PC;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        magicSpawn = transform.root.GetComponentInChildren<Camera>().transform.GetChild(0).transform;
        anim = GetComponent<Animator>();
        PC = transform.root.GetComponent<personController>();
    }



    void CastMagic()
    {
        GameObject newMagic = Instantiate(magicPrefab, magicSpawn.position, Quaternion.identity) as GameObject;
        Rigidbody rb = newMagic.GetComponent<Rigidbody>();
        rb.velocity = magicSpawn.forward * shootForce;
        //newMagic.transform.rotation = transform.rotation;
        //playSound();
    }

    void playSound()
    {
        magicCastSound.Play();
    }
}
