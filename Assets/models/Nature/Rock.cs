using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    int health = 5;
    public AudioSource rockHit;

    void hitObject(int type)
    {
        if(type == 3 || type == 100)
        {
            health--;
            transform.localScale *= .9f;
            rockHit.Play();
            if(type == 3)
            {
                //updateRessources
                gameStatistic.GS.collectStone(2);
                gameStatistic.GS.updateText();
            }      

            if (health <= 0)
            {
                //remove Rock
                int posx = (int)transform.position.x;
                int posz = (int)transform.position.z;
                MapController.MC.changeMapGrid(posx, posz, 0);

                gameStatistic.GS.placedObjects.Remove(gameObject);  //removes this object from global list

                jobsController.JC.rocksToMine.Remove(gameObject);

                Destroy(gameObject, 1f);
                transform.GetChild(0).gameObject.SetActive(false); //only disable the model so that the sound will still play
            }
        }
        
    }

    public bool interactWithObject(GameObject other)
    {
        if (other.GetComponent<personAttributes>().type == 3)
        {
            health--;
            transform.localScale *= .9f;
            rockHit.Play();

            //updateRessources
            gameStatistic.GS.collectStone(2);
            gameStatistic.GS.updateText();


            if (health <= 0)
            {
                //remove Rock
                int posx = (int)transform.position.x;
                int posz = (int)transform.position.z;
                MapController.MC.changeMapGrid(posx, posz, 0);

                gameStatistic.GS.placedObjects.Remove(gameObject);  //removes this object from global list

                jobsController.JC.rocksToMine.Remove(gameObject);

                Destroy(gameObject, 1f);
                transform.GetChild(0).gameObject.SetActive(false); //only disable the model so that the sound will still play
                //jobsController.JC.rocksToMine.Remove(gameObject);
                return true;
            }
        }
        return false;
    }

    public void clickedOnObject()
    {
        if (jobsController.JC.rocksToMine.Contains(gameObject)) return;
        jobsController.JC.rocksToMine.Add(gameObject);
    }
}
