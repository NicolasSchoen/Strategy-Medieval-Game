using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class enemyAttributes : MonoBehaviour
{
    public int type = 0;
    public float damage = 20f;
    public float health = 100f;
    [HideInInspector]
    public float maxhealth;
    public float attackradius = 1f;
    public float chaseRadius = 10;
    public float attackSpeed = 1f;
    public float speed = 1.5f;
    private Vector3 originPosition;
    private GameObject nextPlayer;
    private float distanceToPLayer;
    private Vector3 deltaPosition;
    private bool isReturning = false;
    private Rigidbody rbody;
    Animator anim;
    private float nextAttackTime = 0;
    public bool isDead = false;
    public bool gotHit = false;

    public TextMeshProUGUI txtDamageNr;

    // Start is called before the first frame update
    void Start()
    {
        health *= globalVariables.difficulty;
        maxhealth = health;

        damage *= globalVariables.difficulty;
        damage /= 2;

        originPosition = transform.position;
        rbody = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        gameStatistic.GS.enemys.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        getNearestPlayer();
        if (nextPlayer != null && !isDead)
        {
            distanceToPLayer = Vector3.Distance(nextPlayer.transform.position, transform.position);
            //Debug.Log("distance to player: " + distanceToPLayer.ToString());

            if (distanceToPLayer < attackradius)
            {
                attack();
                gotHit = false;
            }
            else if (distanceToPLayer < chaseRadius || gotHit)
            {
                moveToPlayer();
            }
            else
            {
                moveToBasecamp();
            }
        }
    }

    void applyDamage(float damage)
    {
        health -= damage;
        gotHit = true;

        txtDamageNr.text = damage.ToString();
        txtDamageNr.gameObject.GetComponent<Animator>().SetTrigger("gotHit");

        if (health <= 0)
        {
            anim.SetTrigger("isDead");
            speed = 0;
            transform.GetChild(0).GetComponent<CapsuleCollider>().enabled = false;
            transform.GetComponent<Rigidbody>().useGravity = false;
            destroyEnemy();
        }
    }

    void attack()
    {
        //Debug.Log("enemy attacking player");
        if (Time.time >= nextAttackTime)
        {
            if (anim != null && !isDead) anim.SetTrigger("attack");
            nextAttackTime = Time.time + attackSpeed;
        }
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
        isReturning = false;
        GetComponent<characterMovement>().setReachedTarget();
    }

    void moveToBasecamp()
    {
        if (Vector3.Distance(transform.position, originPosition) < 1)
        {
            if (anim != null) anim.SetBool("isRunning", false);
            return;
        }
        if (isReturning) return;
        //Debug.Log("enemy moving to basecamp");
        GetComponent<characterMovement>().setGoal(originPosition);
        isReturning = true;
        /*if (anim != null) anim.SetBool("isRunning", true);
        deltaPosition = (originPosition - transform.position).normalized * speed * Time.deltaTime;
        //rbody.velocity = new Vector3(0, .8f, 0); //make him jump to pass high slopes
        rbody.position = (rbody.position + deltaPosition);
        transform.rotation = Quaternion.LookRotation(deltaPosition.normalized);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);//keep the person facing up*/
    }

    void getNearestPlayer()
    {
        float nearestCurrent = 0f;
        if (gameStatistic.GS.persons.Count == 0)
        {
            nextPlayer = null;
            return;
        }
        else
        {
            nextPlayer = gameStatistic.GS.persons[0];
            nearestCurrent = Vector3.Distance(nextPlayer.transform.position, transform.position);
        }
        foreach (GameObject player in gameStatistic.GS.persons)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < nearestCurrent)
            {
                nextPlayer = player;
                nearestCurrent = Vector3.Distance(player.transform.position, transform.position);
            }
        }
    }

    void destroyEnemy()
    {
        isDead = true;
        gameStatistic.GS.enemys.Remove(gameObject);
        Destroy(gameObject, 4f);
    }
}
