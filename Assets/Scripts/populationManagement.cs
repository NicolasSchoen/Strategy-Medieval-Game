using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class populationManagement : MonoBehaviour
{
    public GameObject bowArrow;
    public GameObject melee;
    public GameObject civilianM;
    public GameObject civilianF;
    public GameObject builder;
    public GameObject woodcutter;
    public GameObject farmer;
    public GameObject miner;
    public GameObject scout;
    public GameObject knight;
    public GameObject mage;

    public void spawnStartPeople()
    {
        for (int i = 0; i < globalVariables.startWoodcutter; i++)
        {
            Instantiate(woodcutter, new Vector3(globalVariables.mapSize / 2 + Random.Range(0f,1.5f), MapController.MC.getHeight(globalVariables.mapSize / 2, globalVariables.mapSize / 2)+1, globalVariables.mapSize / 2 + 1 + Random.Range(0f, 1.5f)), Quaternion.identity);
        }

        for (int i = 0; i < globalVariables.startMiner; i++)
        {
            Instantiate(miner, new Vector3(globalVariables.mapSize / 2 + Random.Range(0f, 1.5f), MapController.MC.getHeight(globalVariables.mapSize / 2, globalVariables.mapSize / 2) + 1, globalVariables.mapSize / 2 + 1 + Random.Range(0f, 1.5f)), Quaternion.identity);
        }

        for (int i = 0; i < globalVariables.startFarmer; i++)
        {
            Instantiate(farmer, new Vector3(globalVariables.mapSize / 2 + Random.Range(0f, 1.5f), MapController.MC.getHeight(globalVariables.mapSize / 2, globalVariables.mapSize / 2) + 1, globalVariables.mapSize / 2 + 1 + Random.Range(0f, 1.5f)), Quaternion.identity);
        }

        for (int i = 0; i < globalVariables.startBuilder; i++)
        {
            Instantiate(builder, new Vector3(globalVariables.mapSize / 2 + Random.Range(0f, 1.5f), MapController.MC.getHeight(globalVariables.mapSize / 2, globalVariables.mapSize / 2) + 1, globalVariables.mapSize / 2 + 1 + Random.Range(0f, 1.5f)), Quaternion.identity);
        }

        for (int i = 0; i < globalVariables.startSword; i++)
        {
            Instantiate(melee, new Vector3(globalVariables.mapSize / 2 + Random.Range(0f, 1.5f), MapController.MC.getHeight(globalVariables.mapSize / 2, globalVariables.mapSize / 2) + 1, globalVariables.mapSize / 2 + 1 + Random.Range(0f, 1.5f)), Quaternion.identity);
        }

        for (int i = 0; i < globalVariables.startBowarrow; i++)
        {
            Instantiate(bowArrow, new Vector3(globalVariables.mapSize/2 + Random.Range(0f, 1.5f), MapController.MC.getHeight(globalVariables.mapSize / 2, globalVariables.mapSize / 2) + 1, globalVariables.mapSize / 2 + 1 + Random.Range(0f, 1.5f)), Quaternion.identity);
        }
    }

    public void spawnBowArrow()
    {
        if(gameStatistic.GS.beds > gameStatistic.GS.population && gameStatistic.GS.eatFood(50) && gameStatistic.GS.takeIron(2)) Instantiate(bowArrow, new Vector3(transform.parent.position.x,MapController.MC.getHeight(transform.parent.position.x,transform.parent.position.z),transform.parent.position.z), Quaternion.identity);
    }

    public void spawnMelee()
    {
        if (gameStatistic.GS.beds > gameStatistic.GS.population && gameStatistic.GS.eatFood(80) && gameStatistic.GS.takeIron(10)) Instantiate(melee, new Vector3(transform.parent.position.x, MapController.MC.getHeight(transform.parent.position.x, transform.parent.position.z), transform.parent.position.z), Quaternion.identity);
    }

    public void spawnCivilian()
    {
        if (gameStatistic.GS.beds > gameStatistic.GS.population && gameStatistic.GS.eatFood(40))
        {
            if (Random.Range(0, 2) < 1)
            {
                Instantiate(civilianM, new Vector3(transform.parent.position.x, MapController.MC.getHeight(transform.parent.position.x, transform.parent.position.z), transform.parent.position.z), Quaternion.identity);
            }
            else
            {
                Instantiate(civilianF, new Vector3(transform.parent.position.x, MapController.MC.getHeight(transform.parent.position.x, transform.parent.position.z), transform.parent.position.z), Quaternion.identity);
            }
        }        
    }

    public void spawnBuilder()
    {
        if (gameStatistic.GS.beds > gameStatistic.GS.population && gameStatistic.GS.eatFood(40))
        {
            Instantiate(builder, new Vector3(transform.parent.position.x, MapController.MC.getHeight(transform.parent.position.x, transform.parent.position.z), transform.parent.position.z), Quaternion.identity);
        }
    }

    public void spawnWoodcutter()
    {
        if (gameStatistic.GS.beds > gameStatistic.GS.population && gameStatistic.GS.eatFood(40))
        {
            Instantiate(woodcutter, new Vector3(transform.parent.position.x, MapController.MC.getHeight(transform.parent.position.x, transform.parent.position.z), transform.parent.position.z), Quaternion.identity);
        }
    }

    public void spawnFarmer()
    {
        if (gameStatistic.GS.beds > gameStatistic.GS.population && gameStatistic.GS.eatFood(40))
        {
            Instantiate(farmer, new Vector3(transform.parent.position.x, MapController.MC.getHeight(transform.parent.position.x, transform.parent.position.z), transform.parent.position.z), Quaternion.identity);
        }
    }

    public void spawnMiner()
    {
        if (gameStatistic.GS.beds > gameStatistic.GS.population && gameStatistic.GS.eatFood(40))
        {
            Instantiate(miner, new Vector3(transform.parent.position.x, MapController.MC.getHeight(transform.parent.position.x, transform.parent.position.z), transform.parent.position.z), Quaternion.identity);
        }
    }

    public void spawnScout()
    {
        if (gameStatistic.GS.beds > gameStatistic.GS.population && gameStatistic.GS.eatFood(50))
        {
            Instantiate(scout, new Vector3(transform.parent.position.x, MapController.MC.getHeight(transform.parent.position.x, transform.parent.position.z), transform.parent.position.z), Quaternion.identity);
        }
    }

    public void spawnKnight()
    {
        if (gameStatistic.GS.beds > gameStatistic.GS.population && gameStatistic.GS.eatFood(100) && gameStatistic.GS.takeIron(20))
        {
            Instantiate(knight, new Vector3(transform.parent.position.x, MapController.MC.getHeight(transform.parent.position.x, transform.parent.position.z), transform.parent.position.z), Quaternion.identity);
        }
    }

    public void spawnMage()
    {
        if (gameStatistic.GS.beds > gameStatistic.GS.population && gameStatistic.GS.eatFood(200) && gameStatistic.GS.takeIron(100))
        {
            Instantiate(mage, new Vector3(transform.parent.position.x, MapController.MC.getHeight(transform.parent.position.x, transform.parent.position.z), transform.parent.position.z), Quaternion.identity);
        }
    }
}
