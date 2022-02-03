using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    int health = 2;
    bool isCollected = false;
    private float deltaTime;
    private bool isGrown = true;
    private float growRate = .01f;//1% per second
    public AudioSource treeFall;
    public AudioSource treeHit;

    private void Update()
    {
        if (!isGrown)
        {
            if (deltaTime <= 0)
            {
                deltaTime = 1;
                transform.localScale += new Vector3(growRate, growRate, growRate);
                if (transform.localScale.y >= 1) isGrown = true;
            }
            else
            {
                deltaTime -= Time.deltaTime;
            }

        }
    }

    public void startGrow()
    {
        isGrown = false;
        transform.localScale = new Vector3(0, 0, 0);
    }

    public bool getIsCollected()
    {
        return isCollected;
    }

    public bool hitObject(int type)
    {
        if(type == 2 || type == 100)
        {
            health--;
            treeHit.Play();
            if (health <= 0 && !isCollected)
            {
                if(type == 2)
                {
                    //updateRessources
                    isCollected = true;
                    if(isGrown)gameStatistic.GS.collectWood(5);
                    else gameStatistic.GS.collectWood(2);
                    gameStatistic.GS.updateText();
                }

                //remove Tree
                treeFall.Play();
                int posx = (int)transform.position.x;
                int posz = (int)transform.position.z;
                MapController.MC.changeMapGrid(posx, posz, 0);
                gameObject.AddComponent<Rigidbody>();
                gameStatistic.GS.placedObjects.Remove(gameObject);  //removes this object from global list
                Destroy(gameObject, 5.0f);
                return true;
            }
        }
        
        return false;
    }

    public bool interactWithObject(GameObject other)
    {
        if (other.GetComponent<personAttributes>().type == 2)
        {
            health--;
            treeHit.Play();
            if (health <= 0 && !isCollected)
            {

                //updateRessources
                isCollected = true;
                if (isGrown) gameStatistic.GS.collectWood(5);
                else gameStatistic.GS.collectWood(2);
                gameStatistic.GS.updateText();

                //remove Tree
                treeFall.Play();
                int posx = (int)transform.position.x;
                int posz = (int)transform.position.z;
                MapController.MC.changeMapGrid(posx, posz, 0);
                gameObject.AddComponent<Rigidbody>();
                gameStatistic.GS.placedObjects.Remove(gameObject);  //removes this object from global list
                //jobsController.JC.treesToCut.Remove(gameObject);
                Destroy(gameObject, 5.0f);
                return true;
            }
        }
        return false;
    }

    public void clickedOnObject()
    {
        if (jobsController.JC.rocksToMine.Contains(gameObject)) return;
        jobsController.JC.treesToCut.Add(gameObject);
    }
}
