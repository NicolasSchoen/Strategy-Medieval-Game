using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    private float deltaTime;
    private bool isGrown = true;
    public int foodAmount = 5;
    [Tooltip("growrate in Percent per Second")]
    public float growRate = .01f;//1% per second


    private void Update()
    {
        if (!isGrown)
        {
            if(deltaTime <= 0)
            {
                deltaTime = 1;
                transform.localScale += new Vector3(0, growRate, 0);
                if (transform.localScale.y >= 1)
                {
                    setGrown();
                }
            }
            else
            {
                deltaTime -= Time.deltaTime;
            }
            
        }
    }

    public void setGrown()
    {
        isGrown = true;
        jobsController.JC.fieldsToHarvest.Add(gameObject);
    }

    public void startGrow()
    {
        isGrown = false;
        transform.localScale = new Vector3(1, 0, 1);
    }
    public bool interactWithObject(GameObject other)
    {
        if(other.GetComponent<personAttributes>().type == 4)
        {
            int collectedFood = 0;
            if (isGrown) collectedFood = foodAmount;
            gameStatistic.GS.collectFood(collectedFood);
            gameStatistic.GS.updateText();

            //remove Tree
            int posx = (int)transform.position.x;
            int posz = (int)transform.position.z;
            MapController.MC.changeMapGrid(posx, posz, 0);

            gameStatistic.GS.placedObjects.Remove(gameObject);  //removes this object from global list
            //jobsController.JC.fieldsToHarvest.Remove(gameObject);
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    public void clickedOnObject()
    {
        
    }
}
