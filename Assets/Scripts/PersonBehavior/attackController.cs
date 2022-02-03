using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class attackController : MonoBehaviour
{
    public float attackradius = 1f;
    public float chaseRadius = 10;
    public float attackSpeed = 1f;
    public float speed = 1.5f;
    private GameObject nextPlayer;
    private GameObject nextEnemy;
    private float distanceToPLayer;
    private Vector3 deltaPosition;
    private Rigidbody rbody;
    Animator anim;
    private float nextAttackTime = 0;

    private Toggle defendToggle;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        defendToggle = GameObject.Find("ToggleDefend").GetComponent<Toggle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<personController>().isControlled)
        {
            if (nextPlayer != null && !nextPlayer.GetComponentInParent<enemyAttributes>().isDead)
            {
                distanceToPLayer = Vector3.Distance(nextPlayer.transform.position, transform.position);
                //Debug.Log("distance to player: " + distanceToPLayer.ToString());

                if (distanceToPLayer < attackradius)
                {
                    attack();
                }
                else
                {
                    moveToPlayer();
                }
            }
            else
            {
                if (getNearestEnemy())
                {
                    distanceToPLayer = Vector3.Distance(nextEnemy.transform.position, transform.position);
                    //Debug.Log("distance to enemy: " + distanceToPLayer.ToString() + nextEnemy.name);

                    if (nextEnemy != null && (distanceToPLayer < attackradius))
                    {
                        if (defendToggle.isOn)
                        {
                            defend();
                        }
                    }
                }
            }
        }
    }

    void attack()
    {
        //Debug.Log("player attacking enemy");
        anim.SetBool("isRunning", false);
        transform.LookAt(nextPlayer.transform);
        if (Time.time >= nextAttackTime)
        {
            if (anim != null && !GetComponent<personController>().isDead) anim.SetTrigger("attack");
            nextAttackTime = Time.time + attackSpeed;
            //nextPlayer.SendMessageUpwards("applyDamage", damage);
        }
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);//keep the person facing up
    }

    void defend()
    {
        //Debug.Log("player defending");
        anim.SetBool("isRunning", false);
        transform.LookAt(nextEnemy.transform);
        if (Time.time >= nextAttackTime)
        {
            if (anim != null && !GetComponent<personController>().isDead) anim.SetTrigger("attack");
            nextAttackTime = Time.time + attackSpeed;
            //nextPlayer.SendMessageUpwards("applyDamage", damage);
        }
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);//keep the person facing up
    }

    void moveToPlayer()
    {
        //Debug.Log("enemy moving to player");
        if (anim != null) anim.SetBool("isRunning", true);
        deltaPosition = (nextPlayer.transform.position - transform.position).normalized * speed * Time.deltaTime;
        //rbody.velocity = new Vector3(0, .8f, 0); //make him jump to pass high slopes
        rbody.position = (rbody.position + deltaPosition);
        transform.rotation = Quaternion.LookRotation(deltaPosition.normalized);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);//keep the person facing up
        GetComponent<characterMovement>().setReachedTarget();
    }

    bool getNearestEnemy()
    {
        float nearestCurrent = 0f;
        if (gameStatistic.GS.enemys.Count == 0)
        {
            nextEnemy = null;
            return false;
        }
        else
        {
            nextEnemy = gameStatistic.GS.enemys[0];
            nearestCurrent = Vector3.Distance(nextEnemy.transform.position, transform.position);
        }
        foreach (GameObject enemy in gameStatistic.GS.enemys)
        {
            if(enemy != null)
            if (Vector3.Distance(enemy.transform.position, transform.position) < nearestCurrent)
            {
                nextEnemy = enemy;
                nearestCurrent = Vector3.Distance(enemy.transform.position, transform.position);
            }
        }
        return true;
    }

    public void setEnemy(GameObject selectedEnemy)
    {
        nextPlayer = selectedEnemy;
    }

    public void removeEnemy()
    {
        nextPlayer = null;
    }
}
