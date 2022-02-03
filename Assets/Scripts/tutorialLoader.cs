using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tutorialLoader : MonoBehaviour
{
    public int level = 0;
    public int wood = 0;
    public int stone = 0;
    public int iron = 0;
    public int food = 0;

    public int woodcutter = 0;
    public int farmer = 0;
    public int miner = 0;
    public int builder = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadLevel()
    {
        globalVariables.startWood = wood;
        globalVariables.startStone = stone;
        globalVariables.startIron = iron;
        globalVariables.startFood = food;

        globalVariables.startWoodcutter = woodcutter;
        globalVariables.startFarmer = farmer;
        globalVariables.startMiner = miner;
        globalVariables.startBuilder = builder;
        SceneManager.LoadScene("buildingGenerator");
    }
}
