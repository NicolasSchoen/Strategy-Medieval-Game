using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class woodcutterController : MonoBehaviour
{
    public GameObject currentTree;
    public bool isWorking = true;
    private Animator anim;
    //public object isControlled;

    public float harvestDistance = .5f;
    public float workDistance = 10f;

    public float attackRatePerSecond = 1f;
    float nextAttackTime = 0f;

    private bool reachedTree = false;
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
                if (currentTree == null)
                {
                    reachedTree = false;//TESTING
                    GetComponent<Rigidbody>().useGravity = true;
                    if (jobsController.JC.treesToCut.Count > 0)
                    {
                        getNearestTree();
                        GetComponent<characterMovement>().setGoal(currentTree.transform.position);
                    }

                }
                else
                {
                    if ((currentTree.transform.position - transform.position).magnitude < harvestDistance)
                    {
                        reachedTree = true;
                        GetComponent<Rigidbody>().useGravity = false;
                        GetComponent<Rigidbody>().velocity *= 0;
                    }
                    if (reachedTree)
                    {
                        if (!currentTree.GetComponent<Tree>().getIsCollected())
                        {
                            if (anim != null) anim.SetTrigger("attack");
                            nextAttackTime = Time.time + 1f / attackRatePerSecond;
                            //TODO: wenn baum durch meleehit bereits gefaellt, wird das nicht erkannt
                            //evtl. kann eine weitere animation ohne das trigger-event hier helfen.
                        }
                        else//if (currentTree.GetComponent<Tree>().interactWithObject(gameObject))
                        {
                            reachedTree = false;
                            currentTree = null;
                            GetComponent<Rigidbody>().useGravity = true;
                        }
                    }
                }
            }
        }
    }

    void getNearestTree()
    {
        float nearestCurrent = 0f;
        if (jobsController.JC.treesToCut.Count == 0)
        {
            currentTree = null;
            return;
        }
        else
        {
            currentTree = jobsController.JC.treesToCut[0];
            nearestCurrent = Vector3.Distance(currentTree.transform.position, transform.position);
        }
        foreach (GameObject tree in jobsController.JC.treesToCut)
        {
            if (Vector3.Distance(tree.transform.position, transform.position) < nearestCurrent)
            {
                currentTree = tree;
                nearestCurrent = Vector3.Distance(tree.transform.position, transform.position);
            }
        }

        jobsController.JC.treesToCut.Remove(currentTree);
    }

    public void DropJob()
    {
        if(currentTree != null)
        {
            jobsController.JC.treesToCut.Add(currentTree);
            currentTree = null;
        }
    }
}
