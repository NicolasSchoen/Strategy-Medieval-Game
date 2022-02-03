using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour
{
    private bool hitObject = false;
    public bool isExplosive = false;
    public float explosionRadius = 2f;
    public int damage = 5;

    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);//bullet only lives for 5 seconds
        if (Random.Range(0, 10) < 2) isExplosive = true;//20% change zu explodieren
    }

    // Update is called once per frame
    /*void Update()
    {

    }*/

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bullet Hit: " + collision.gameObject.tag);
        if(collision.gameObject.tag == "ground" && !hitObject && isExplosive)
        {
            collideTerrain(collision);
        }
        if(collision.gameObject.tag == "nature" && !hitObject)
        {
            collision.gameObject.SendMessageUpwards("hitObject",100);
            Destroy(gameObject);
            hitObject = true;
        }
        if ((collision.gameObject.tag == "person" || collision.gameObject.tag == "military") && !hitObject)
        {
            collision.gameObject.SendMessageUpwards("applyDamage", damage + Random.Range(0, 5));
            Destroy(gameObject);
            hitObject = true;
        }
        if (isExplosive)
        {
            GameObject explosioneffect = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(explosioneffect, 2f);
            //create explosion force
            Collider[] nearbyColliders = Physics.OverlapSphere(transform.position,explosionRadius);

            foreach (Collider nearbyCollider in nearbyColliders)
            {
                Rigidbody rb = nearbyCollider.GetComponentInParent<Rigidbody>();
                personController pc = nearbyCollider.GetComponentInParent<personController>();
                if(rb != null)
                {
                    rb.useGravity = true;
                    rb.AddExplosionForce(50,transform.position, explosionRadius);
                }
                if(pc != null)
                {
                    pc.applyDamage(damage);
                }

            }

            Destroy(gameObject);
        }
        //Destroy(gameObject);
    }

    private void collideTerrain(Collision collision)
    {
        object[] tempStorage = new object[3];
        tempStorage[0] = collision.GetContact(collision.contactCount - 1).point;
        tempStorage[1] = -.5f;
        tempStorage[2] = 1;
        collision.gameObject.SendMessage("changeTerrain", tempStorage);
        //hitObject = true;
    }
}
