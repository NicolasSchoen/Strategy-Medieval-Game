using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceLance : MonoBehaviour
{
    Rigidbody rb;
    public float lifeTime = 5f;
    //private float timer;
    private bool hitSomething = false;

    public GameObject iceShatterHitEffect;
    //public AudioSource iceImpactSound;

    public float magicDamage;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.rotation = Quaternion.LookRotation(rb.velocity);
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (!hitSomething)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hitSomething) return;
        if (collision.collider.tag != "person" && collision.collider.tag != "military")
        {
            hitSomething = true;
            
            if (iceShatterHitEffect != null)
            {
                //iceImpactSound.Play();
                GameObject HitEffect = Instantiate(iceShatterHitEffect, transform.position, transform.rotation);
                Destroy(HitEffect, 0.5f);
                Destroy(gameObject);
            }
            

            if (collision.collider.tag == "enemy")
            {
                //Deal damage
                collision.gameObject.SendMessageUpwards("applyDamage", magicDamage + Random.Range(0, 5));
            }
        }
    }
}
