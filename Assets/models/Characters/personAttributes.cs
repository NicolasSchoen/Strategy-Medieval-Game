using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class personAttributes : MonoBehaviour
{
    public float health = 10;
    [HideInInspector]
    public float maxhealth;
    public float armor = 0;
    [HideInInspector]
    public float maxarmor;
    public float damage = 0;
    public int type = 1;//1: builder, 2: woodcutter, 3 :miner, 4: farmer, 5: swordfighter, 6: bowarrow, 7: scout
    // Start is called before the first frame update
    void Start()
    {
        maxhealth = health;
        maxarmor = armor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getJobName()
    {
        string jobString = "";
        switch (type)
        {
            case 1:
                {
                    jobString = "Builder";
                    break;
                }
            case 2:
                {
                    jobString = "Woodcutter";
                    break;
                }
            case 3:
                {
                    jobString = "Miner";
                    break;
                }
            case 4:
                {
                    jobString = "Farmer";
                    break;
                }
            case 5:
                {
                    jobString = "Swordsman";
                    break;
                }
            case 6:
                {
                    jobString = "Archer";
                    break;
                }
            case 7:
                {
                    jobString = "Scout";
                    break;
                }
            case 8:
                {
                    jobString = "Knight";
                    break;
                }
            case 9:
                {
                    jobString = "Wizard";
                    break;
                }
        }
        return jobString;
    }
}
