using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHit : MonoBehaviour
{
    public AudioSource meleeHitSound;
    public Transform attackPoint;
    public float attackRange = 0.5f;

    public GameObject ObjectHitEffect;
    public GameObject enemyHitEffect;

    public bool isEnemy = false;    


    void ObjectHitEvent()
    {
        Collider[] hitColliders = Physics.OverlapSphere(attackPoint.position, attackRange);

        foreach(Collider hit in hitColliders)
        {
            if(hit.tag == "nature" && tag == "person")
            {
                Debug.Log("Hit: " + hit.name);
                hit.gameObject.SendMessageUpwards("hitObject", GetComponentInParent<personAttributes>().type);

                if(ObjectHitEffect != null)
                {
                    GameObject HitEffect = Instantiate(ObjectHitEffect, attackPoint.position, attackPoint.rotation);
                    Destroy(HitEffect, 0.5f);
                }
                playHitSound();
                return;
            }

            if (isEnemy)
            {
                if (hit.tag == "person" || hit.tag == "military")
                {
                    Debug.Log("Hit: " + hit.name);
                    hit.gameObject.SendMessageUpwards("applyDamage", GetComponentInParent<enemyAttributes>().damage + Random.Range(0, 5));

                    if (enemyHitEffect != null)
                    {
                        GameObject HitEffect = Instantiate(enemyHitEffect, attackPoint.position, attackPoint.rotation);
                        Destroy(HitEffect, 0.5f);
                    }
                    playHitSound();
                    return;
                }
            }
            else
            {
                if (hit.tag == "enemy")
                {
                    Debug.Log("Hit: " + hit.name);
                    hit.gameObject.SendMessageUpwards("applyDamage", GetComponentInParent<personAttributes>().damage + Random.Range(0, 5));

                    if (enemyHitEffect != null)
                    {
                        GameObject HitEffect = Instantiate(enemyHitEffect, attackPoint.position, attackPoint.rotation);
                        Destroy(HitEffect, 0.5f);
                    }
                    playHitSound();
                    return;
                }
            }
        }        
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void playHitSound()
    {
        meleeHitSound.Play();
    }

    void SwordTrailOn()
    {
        attackPoint.GetChild(0).gameObject.SetActive(true);
    }
    void SwordTrailOff()
    {
        attackPoint.GetChild(0).gameObject.SetActive(false);
    }
}
