using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronRock : MonoBehaviour
{
    int ironAmount = 100;
    public AudioSource rockHit;

    void hitObject(int type)
    {
        if (type > 10) return;  //hit by bullet
        if (type == 3 && ironAmount > 0)
        {
            rockHit.Play();
            //updateRessources
            ironAmount--;
            gameStatistic.GS.collectIron(1);
            gameStatistic.GS.updateText();
        }
    }

    public void interactWithObject(GameObject other)
    {

    }

    public void clickedOnObject()
    {

    }
}
