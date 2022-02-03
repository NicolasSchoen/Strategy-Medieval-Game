using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Church : MonoBehaviour
{
    public int ressourcesOverTime = 1;
    public float secondsToCollect = 5.0f;
    private float deltaTime;
    public bool collectRessources = true;

    public AudioSource churchSound;

    private object isPlaced;

    void Start()
    {
        isPlaced = (object)GetComponent<ModelAttributes>().isPlaced;
        if ((bool)isPlaced)
        {
            churchSound.Play();
        }
    }

    private void Update()
    {
        if ((bool)isPlaced)
        {            
            deltaTime -= Time.deltaTime;
            if (deltaTime < 0)
            {
                CollectRessources();
                deltaTime = secondsToCollect;
            }
        }
    }

    void CollectRessources()
    {
        if (collectRessources)
        {
            gameStatistic.GS.collectWood(ressourcesOverTime);
            gameStatistic.GS.collectStone(ressourcesOverTime);
            gameStatistic.GS.collectIron(ressourcesOverTime);
            gameStatistic.GS.collectFood(ressourcesOverTime);
            gameStatistic.GS.updateText();
        }        
    }
}
