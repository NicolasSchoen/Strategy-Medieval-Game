using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class farmerController : MonoBehaviour
{
    public GameObject currentField;
    public bool isWorking = true;
    private Animator anim;
    //public object isControlled;

    private float attackRatePerSecond = .2f;
    float nextAttackTime = 0f;

    public float harvestDistance = .5f;
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
                if (currentField == null)
                {
                    if (jobsController.JC.fieldsToHarvest.Count > 0)
                    {
                        //pick field and harvest it
                        getNearestField();
                        GetComponent<characterMovement>().setGoal(currentField.transform.position);
                    }

                }
                else
                {
                    if ((currentField.transform.position - transform.position).magnitude < harvestDistance)
                    {
                        //harvest the field
                        if (anim != null) anim.SetTrigger("attack");
                        nextAttackTime = Time.time + 1f / attackRatePerSecond;
                        if (currentField.GetComponent<Field>().interactWithObject(gameObject)) currentField = null;
                    }
                }
            }
        }
    }

    void getNearestField()
    {
        float nearestCurrent = 0f;
        if (jobsController.JC.fieldsToHarvest.Count == 0)
        {
            currentField = null;
            return;
        }
        else
        {
            currentField = jobsController.JC.fieldsToHarvest[0];
            nearestCurrent = Vector3.Distance(currentField.transform.position, transform.position);
        }
        foreach (GameObject field in jobsController.JC.fieldsToHarvest)
        {
            if (Vector3.Distance(field.transform.position, transform.position) < nearestCurrent)
            {
                currentField = field;
                nearestCurrent = Vector3.Distance(field.transform.position, transform.position);
            }
        }

        jobsController.JC.fieldsToHarvest.Remove(currentField);
    }
}
