using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class globalVariables
{
    public static string loadedSaveName = "";
    public static bool loadSaveGame = false;
    public static bool isPeaceful = false;
    public static int treeFactor = 1;
    public static int stoneFactor = 1;
    public static int MountainFactor = 1;
    public static int mapSize = 100;
    public static int difficulty = 2;

    public static int startBuilder = 0;
    public static int startWoodcutter = 0;
    public static int startFarmer = 0;
    public static int startMiner = 0;

    public static int startSword = 0;
    public static int startBowarrow = 0;

    public static int startWood = 0;
    public static int startStone = 0;
    public static int startIron = 0;
    public static int startFood = 0;

    public static int returnFactor = 1;    

    public static int possibilityForRain = 10;
    public static bool enableCheating = false;
    public static bool GodMode = false;

    public static void prepareWorld()
    {
        switch (difficulty)
        {
            case 1:
                {
                    //Easy
                    startWood = startStone = 40;
                    startIron = 20;
                    startFood = 100;
                    startFarmer = startWoodcutter = startBuilder = startMiner = 1;
                    startSword = 1;
                    returnFactor = 1;
                    enableCheating = true;
                    break;
                }
            case 2:
                {
                    //Medium
                    startWood = startStone = startIron = 0;
                    startFood = 50;
                    startFarmer = startWoodcutter  = 1;
                    startSword = startBowarrow = startBuilder = startMiner = 0;
                    returnFactor = 2;
                    enableCheating = true;
                    break;
                }
            case 3:
                {
                    //Hard
                    startFood = startWood = startStone = startIron = 0;
                    startFarmer = startWoodcutter = 1;
                    startBuilder = startMiner = 0;
                    startSword = startBowarrow = 0;
                    returnFactor = 10;
                    enableCheating = false;
                    break;
                }
        }
    }
}
