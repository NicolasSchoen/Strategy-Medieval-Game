using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawnerScript : MonoBehaviour
{
    private List<GameObject> spawnedEnemys = new List<GameObject>();
    public GameObject[] enemys;
    public int enemyAmount = 2;
    public int spawnRadius = 2;
    private float nextRespawnTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        spawnEnemys(enemyAmount);
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnedEnemys.Count > 0 && spawnedEnemys[0] == null)
        {
            if (nextRespawnTime <= 0f)
            {
                spawnedEnemys.Clear();
                spawnEnemys(1);
                nextRespawnTime = 5f;
            }
            nextRespawnTime -= Time.deltaTime;
        }
    }

    private void spawnEnemys(int amount)
    {
        float posx, posz;
        int type;
        int errorcount = 1000;

        if (globalVariables.loadSaveGame) return;
        for (int i = 0; i < amount; i++)
        {
            type = Random.Range(0, enemys.Length);
            posx = Random.Range((int)transform.position.x - spawnRadius - 1, (int)transform.position.x + GetComponent<ModelAttributes>().blockWidth + spawnRadius - 1);
            posz = Random.Range((int)transform.position.z - spawnRadius - 1, (int)transform.position.z + GetComponent<ModelAttributes>().blockHeight + spawnRadius - 1);
            if (posx < 0 || posz < 0 || posx >= MapController.MC.mapSize || posz >= MapController.MC.mapSize) { 
                if(errorcount>0)i -= 1;
                errorcount--;
                continue; 
            }
            GameObject spawnedCurrent = Instantiate(enemys[type], new Vector3(posx, MapController.MC.getHeight(posx, posz), posz), Quaternion.identity);
            spawnedEnemys.Add(spawnedCurrent);
            //gameStatistic.GS.enemys.Add(spawnedCurrent);
        }
    }
}
