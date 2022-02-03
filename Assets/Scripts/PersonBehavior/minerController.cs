using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minerController : MonoBehaviour
{
    public GameObject currentRock;
    public bool isWorking = true;
    private Animator anim;
    //public object isControlled;

    public float harvestDistance = 1f;
    public float workDistance = 10f;

    public float attackRatePerSecond = 1f;
    float nextAttackTime = 0f;

    private bool reachedRock = false;
    // Start is called before the first frame update
    void Start()
    {
        //isControlled = (object) GetComponent<personController>().isControlled;
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<personController>().isControlled)
        {
            if (Time.time >= nextAttackTime)         //Anrgiffe auf 1 Angriff pro Sekunde reduzieren
            {
                if (currentRock == null)
                {
                    reachedRock = false;//TESTING
                    GetComponent<Rigidbody>().useGravity = true;
                    if (jobsController.JC.rocksToMine.Count > 0)
                    {
                        //pick rock and mine it
                        getNearestRock();
                        //currentRock = jobsController.JC.rocksToMine[0];
                        //jobsController.JC.rocksToMine.Remove(currentRock);
                        GetComponent<characterMovement>().setGoal(currentRock.transform.position);
                    }

                }
                else
                {
                    if ((currentRock.transform.position - transform.position).magnitude < harvestDistance)
                    {
                        reachedRock = true;
                        GetComponent<Rigidbody>().useGravity = false;
                        GetComponent<Rigidbody>().velocity *= 0;
                    }
                    if (reachedRock)
                    {
                        if (anim != null) anim.SetTrigger("attack");
                        nextAttackTime = Time.time + 1f / attackRatePerSecond;
                    }
                }
            }
        }
    }

    void getNearestRock()
    {
        float nearestCurrent = 0f;
        if (jobsController.JC.rocksToMine.Count == 0)
        {
            currentRock = null;
            return;
        }
        else
        {
            currentRock = jobsController.JC.rocksToMine[0];
            nearestCurrent = Vector3.Distance(currentRock.transform.position, transform.position);
        }
        foreach (GameObject rock in jobsController.JC.rocksToMine)
        {
            if (Vector3.Distance(rock.transform.position, transform.position) < nearestCurrent)
            {
                currentRock = rock;
                nearestCurrent = Vector3.Distance(rock.transform.position, transform.position);
            }
        }

        jobsController.JC.rocksToMine.Remove(currentRock);
    }

    public void DropJob()
    {
        if (currentRock != null)
        {
            jobsController.JC.rocksToMine.Add(currentRock);
            currentRock = null;
        }
    }
}
