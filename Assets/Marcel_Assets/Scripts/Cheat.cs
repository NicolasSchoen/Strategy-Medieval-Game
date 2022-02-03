using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    [Header("CTRL + U for Ressources")]
    public bool cheatActive = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cheatActive)
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.U))
            {
                gameStatistic.GS.collectWood(1000);
                gameStatistic.GS.collectStone(1000);
                gameStatistic.GS.collectIron(1000);
                gameStatistic.GS.collectFood(1000);
                gameStatistic.GS.updateText();
            }
        }        
    }
}
