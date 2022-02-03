using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody rb;
    public float lifeTime = 5f;
    //private float timer;
    private bool hitSomething = false;

    public GameObject enemyHitEffect;
    public AudioSource arrowFleshImpact;

    public float arrowDamage;

    public bool isSticky = true;

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
        /*timer += Time.deltaTime;
        if(timer >= lifeTime)
        {
            Destroy(gameObject,lifeTime);
        }*/

        if (!hitSomething)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hitSomething) return;
        if(collision.collider.tag != "arrow" && collision.collider.tag != "person")
        {
            hitSomething = true;
            if(isSticky) Stick();
            if(collision.collider.tag == "enemy")
            {
                if (enemyHitEffect != null)
                {
                    GameObject HitEffect = Instantiate(enemyHitEffect, transform.position, transform.rotation);
                    Destroy(HitEffect, 0.5f);
                }
                arrowFleshImpact.Play();
                transform.parent = collision.transform;
                //Deal damage
                collision.gameObject.SendMessageUpwards("applyDamage", arrowDamage + Random.Range(0, 5));
            }
        }
    }

    private void Stick()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        //rb.velocity = Vector3.zero;
        //rb.useGravity = false;
    }
}
