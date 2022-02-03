using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class savegameController : MonoBehaviour
{
    public TMP_InputField filename;
    public Canvas saveCanvas;

    public GameObject tree;
    public GameObject tree2;
    public GameObject tree3;
    public GameObject rock;
    public GameObject ironMountain;
    public GameObject field;
    public GameObject cornField;
    public GameObject simpleTower;
    public GameObject stoneTower;
    public GameObject fortress;
    public GameObject woodHouse;
    public GameObject stoneHouse;
    public GameObject smallHouse;
    public GameObject bigHouse;
    public GameObject farm;
    public GameObject woodFence;
    public GameObject woodFenceCorner;
    public GameObject stoneFence;
    public GameObject stoneFenceCorner;
    public GameObject camp;
    public GameObject catapult;
    public GameObject enemyCamp;
    public GameObject castle;
    public GameObject forestry;
    public GameObject mine;
    public GameObject stonemine;
    public GameObject foodStorage;
    public GameObject resourceStorage;
    public GameObject church;
    public GameObject streetLamp;
    public GameObject well;
    public GameObject watchTower;
    public GameObject largeTower;
    public GameObject blacksmith;
    public GameObject butcher;
    public GameObject cityWall;
    public GameObject wallGate;
    public GameObject shootingRange;
    public GameObject mageTower;

    public GameObject civilianM;
    public GameObject civilianF;
    public GameObject builder;
    public GameObject woodcutter;
    public GameObject miner;
    public GameObject farmer;
    public GameObject scout;
    public GameObject swordfighter;
    public GameObject bowarrow;
    public GameObject knight;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public void saveToFile()
    {
        Debug.Log("filename: " + filename.text);
        if(filename == null || filename.text.Length <= 0)
        {
            return;
        }
        string saveFileName = filename.text.Trim();
        globalVariables.loadedSaveName = saveFileName;
        BuildingData[] buildingData = new BuildingData[gameStatistic.GS.placedObjects.Count];
        PersonData[] personData = new PersonData[gameStatistic.GS.persons.Count];
        EnemyData[] enemyData = new EnemyData[gameStatistic.GS.enemys.Count];
        MapData mapData = new MapData();

        //TODO: check if the savefile already exists, send overwrite warning
        //create the save directory
        Directory.CreateDirectory(Application.dataPath + "/saves/" + saveFileName);
        //Persons
        int i = 0;
        foreach (GameObject person in gameStatistic.GS.persons)
        {
            personData[i] = new PersonData();
            personData[i].name = person.name;
            personData[i].health = person.GetComponent<personAttributes>().health;
            personData[i].type = person.GetComponent<personAttributes>().type;
            //position
            personData[i].posx = person.transform.position.x;
            personData[i].posy = person.transform.position.y;
            personData[i].posz = person.transform.position.z;

            //rotation
            personData[i].rotx = person.transform.rotation.eulerAngles.x;
            personData[i].roty = person.transform.rotation.eulerAngles.y;
            personData[i].rotz = person.transform.rotation.eulerAngles.z;
            i++;

            //string jsonperson = JsonUtility.ToJson(personData);
            //Debug.Log(jsonperson);
        }
        string jsonpersonlist = JsonHelper.ToJson(personData,true);
        Debug.Log(jsonpersonlist);

        //if(File.Exists(Application.dataPath + "/saves/" + saveFileName + "/persons.save"))File.Delete(Application.dataPath + "/saves/" + saveFileName + "/persons.save");
        File.WriteAllText(Application.dataPath + "/saves/" + saveFileName + "/persons.save",jsonpersonlist);

        i = 0;
        //Buildings
        foreach (GameObject building in gameStatistic.GS.placedObjects)
        {
            buildingData[i] = new BuildingData();
            buildingData[i].name = building.name;
            buildingData[i].type = building.GetComponent<ModelAttributes>().modelType;
            buildingData[i].subtype = building.GetComponent<ModelAttributes>().subtype;
            buildingData[i].width = building.GetComponent<ModelAttributes>().blockWidth;
            buildingData[i].height = building.GetComponent<ModelAttributes>().blockHeight;
            //position
            buildingData[i].posx = building.transform.position.x;
            buildingData[i].posy = building.transform.position.y;
            buildingData[i].posz = building.transform.position.z;

            //rotation
            buildingData[i].rotx = building.transform.rotation.eulerAngles.x;
            buildingData[i].roty = building.transform.rotation.eulerAngles.y;
            buildingData[i].rotz = building.transform.rotation.eulerAngles.z;
            i++;
            
        }
        string jsonbuilding = JsonHelper.ToJson(buildingData,true);
        Debug.Log(jsonbuilding);
        File.WriteAllText(Application.dataPath + "/saves/" + saveFileName + "/buildings.save", jsonbuilding);

        //load data: BuildingData[] buildingData = JsonHelper.FromJson<BuildingData>(jsonbuilding);
        //Enemys
        i = 0;
        foreach (GameObject enemy in gameStatistic.GS.enemys)
        {
            enemyData[i] = new EnemyData();
            enemyData[i].health = enemy.GetComponent<enemyAttributes>().health;
            enemyData[i].type = enemy.GetComponent<enemyAttributes>().type;
            //position
            enemyData[i].posx = enemy.transform.position.x;
            enemyData[i].posy = enemy.transform.position.y;
            enemyData[i].posz = enemy.transform.position.z;

            //rotation
            enemyData[i].rotx = enemy.transform.rotation.eulerAngles.x;
            enemyData[i].roty = enemy.transform.rotation.eulerAngles.y;
            enemyData[i].rotz = enemy.transform.rotation.eulerAngles.z;
            i++;

        }
        string jsonenemy = JsonHelper.ToJson(enemyData, true);
        Debug.Log(jsonenemy);
        File.WriteAllText(Application.dataPath + "/saves/" + saveFileName + "/enemys.save", jsonenemy);

        //Map
        mapData.collectedWood = gameStatistic.GS.getWood();
        mapData.collectedStone = gameStatistic.GS.getStone();
        mapData.collectedIron = gameStatistic.GS.getIron();
        mapData.collectedFood = gameStatistic.GS.getFood();
        mapData.availableBeds = gameStatistic.GS.beds;
        mapData.size = MapController.MC.mapSize;
        mapData.mapgrid = MapController.MC.getSerializedMapGrid();
        mapData.vertexes = MapController.MC.getVertexes();
        mapData.tris = MapController.MC.getTris();
        string jsonmap = JsonUtility.ToJson(mapData, true);
        Debug.Log(jsonmap);
        File.WriteAllText(Application.dataPath + "/saves/" + saveFileName + "/map.save", jsonmap);

        saveCanvas.enabled = false;
    }

    public bool loadFromFile()
    {

        if (globalVariables.loadedSaveName.Length <= 0) return false;
        if(File.Exists(Application.dataPath + "/saves/" + globalVariables.loadedSaveName + "/map.save"))
        {
            string mapJson = File.ReadAllText(Application.dataPath + "/saves/" + globalVariables.loadedSaveName + "/map.save");
            string buildingJson = File.ReadAllText(Application.dataPath + "/saves/" + globalVariables.loadedSaveName + "/buildings.save");
            string personJson = File.ReadAllText(Application.dataPath + "/saves/" + globalVariables.loadedSaveName + "/persons.save");
            string enemyJson = File.ReadAllText(Application.dataPath + "/saves/" + globalVariables.loadedSaveName + "/enemys.save");
            MapData newMap = JsonUtility.FromJson<MapData>(mapJson);
            BuildingData[] buildingData = JsonHelper.FromJson<BuildingData>(buildingJson);
            PersonData[] personData = JsonHelper.FromJson<PersonData>(personJson);
            EnemyData[] enemyData = JsonHelper.FromJson<EnemyData>(enemyJson);

            //reconstruct map
            globalVariables.mapSize = newMap.size;
            MapController.MC.loadMap(newMap.size, newMap.mapgrid, newMap.tris, newMap.vertexes);
            gameStatistic.GS.wood = newMap.collectedWood;
            gameStatistic.GS.stone = newMap.collectedStone;
            gameStatistic.GS.iron = newMap.collectedIron;
            gameStatistic.GS.food = newMap.collectedFood;
            gameStatistic.GS.beds = newMap.availableBeds;
            gameStatistic.GS.updateText();

            //place objects
            Vector3 placePosition;
            Vector3 eulerRotation;
            foreach (BuildingData building in buildingData)
            {
                placePosition = new Vector3(building.posx, building.posy, building.posz);
                eulerRotation = new Vector3(building.rotx, building.roty, building.rotz);
                GameObject placedFromSavefile;
                switch (building.type)
                {
                    case 1:
                        {
                            if (building.subtype == 0)
                            {
                                placedFromSavefile = Instantiate(tree, placePosition, Quaternion.Euler(eulerRotation));
                                placedFromSavefile.GetComponent<ModelAttributes>().placeObject();                               
                                gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            }
                            else if (building.subtype == 1)
                            {
                                placedFromSavefile = Instantiate(tree2, placePosition, Quaternion.Euler(eulerRotation));
                                placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                                gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            }
                            else if (building.subtype == 2)
                            {
                                placedFromSavefile = Instantiate(tree3, placePosition, Quaternion.Euler(eulerRotation));
                                placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                                gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            }
                            break;
                        }
                    case 2:
                        {
                            if(building.subtype == 0)
                            {
                                placedFromSavefile = Instantiate(field, placePosition, Quaternion.Euler(eulerRotation));
                                placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                                placedFromSavefile.GetComponent<Field>().setGrown();
                                gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            }
                            else if(building.subtype == 1)
                            {
                                placedFromSavefile = Instantiate(cornField, placePosition, Quaternion.Euler(eulerRotation));
                                placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                                placedFromSavefile.GetComponent<Field>().setGrown();
                                gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            }
                            break;
                        }
                    case 3:
                        {
                            placedFromSavefile = Instantiate(ironMountain, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 4:
                        {
                            placedFromSavefile = Instantiate(rock, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 10:
                        {
                            placedFromSavefile = Instantiate(simpleTower, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 11:
                        {
                            placedFromSavefile = Instantiate(stoneTower, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 12:
                        {
                            placedFromSavefile = Instantiate(fortress, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 13:
                        {
                            placedFromSavefile = Instantiate(enemyCamp, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 14:
                        {
                            placedFromSavefile = Instantiate(castle, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 20:
                        {
                            placedFromSavefile = Instantiate(woodHouse, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 21:
                        {
                            placedFromSavefile = Instantiate(stoneHouse, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 22:
                        {
                            placedFromSavefile = Instantiate(smallHouse, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 23:
                        {
                            placedFromSavefile = Instantiate(bigHouse, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 24:
                        {
                            placedFromSavefile = Instantiate(forestry, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 25:
                        {
                            placedFromSavefile = Instantiate(farm, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 26:
                        {
                            placedFromSavefile = Instantiate(mine, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 27:
                        {
                            placedFromSavefile = Instantiate(stonemine, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 28:
                        {
                            placedFromSavefile = Instantiate(foodStorage, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 29:
                        {
                            placedFromSavefile = Instantiate(resourceStorage, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 30:
                        {
                            placedFromSavefile = Instantiate(woodFence, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 31:
                        {
                            placedFromSavefile = Instantiate(woodFenceCorner, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 35:
                        {
                            placedFromSavefile = Instantiate(stoneFence, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 36:
                        {
                            placedFromSavefile = Instantiate(stoneFenceCorner, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 50:
                        {
                            placedFromSavefile = Instantiate(camp, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 60:
                        {
                            placedFromSavefile = Instantiate(catapult, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 61:
                        {
                            placedFromSavefile = Instantiate(streetLamp, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 62:
                        {
                            placedFromSavefile = Instantiate(well, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 70:
                        {
                            placedFromSavefile = Instantiate(church, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 72:
                        {
                            placedFromSavefile = Instantiate(watchTower, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 73:
                        {
                            placedFromSavefile = Instantiate(largeTower, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 75:
                        {
                            placedFromSavefile = Instantiate(blacksmith, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 76:
                        {
                            placedFromSavefile = Instantiate(butcher, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 77:
                        {
                            placedFromSavefile = Instantiate(cityWall, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 78:
                        {
                            placedFromSavefile = Instantiate(wallGate, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 79:
                        {
                            placedFromSavefile = Instantiate(shootingRange, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }
                    case 89:
                        {
                            placedFromSavefile = Instantiate(mageTower, placePosition, Quaternion.Euler(eulerRotation));
                            placedFromSavefile.GetComponent<ModelAttributes>().placeObject();
                            gameStatistic.GS.placedObjects.Add(placedFromSavefile);
                            break;
                        }


                }
            }

            //place persons
            gameStatistic.GS.removeAllPersons();
            foreach (PersonData person in personData)
            {
                placePosition = new Vector3(person.posx, person.posy, person.posz);
                eulerRotation = new Vector3(person.rotx, person.roty, person.rotz);
                switch (person.type)
                {
                    case 1:
                        {
                            //1: builder
                            GameObject tmpPerson = Instantiate(builder, placePosition, Quaternion.Euler(eulerRotation));
                            tmpPerson.GetComponent<personAttributes>().health = person.health;
                            tmpPerson.GetComponent<personAttributes>().type = person.type;
                            break;
                        }
                    case 2:
                        {
                            //woodcutter
                            GameObject tmpPerson = Instantiate(woodcutter, placePosition, Quaternion.Euler(eulerRotation));
                            tmpPerson.GetComponent<personAttributes>().health = person.health;
                            tmpPerson.GetComponent<personAttributes>().type = person.type;
                            break;
                        }
                    case 3:
                        {
                            //miner
                            GameObject tmpPerson = Instantiate(miner, placePosition, Quaternion.Euler(eulerRotation));
                            tmpPerson.GetComponent<personAttributes>().health = person.health;
                            tmpPerson.GetComponent<personAttributes>().type = person.type;
                            break;
                        }
                    case 4:
                        {
                            //farmer
                            GameObject tmpPerson = Instantiate(farmer, placePosition, Quaternion.Euler(eulerRotation));
                            tmpPerson.GetComponent<personAttributes>().health = person.health;
                            tmpPerson.GetComponent<personAttributes>().type = person.type;
                            break;
                        }
                    case 5:
                        {
                            //swordfighter;
                            GameObject tmpPerson = Instantiate(swordfighter, placePosition, Quaternion.Euler(eulerRotation));
                            tmpPerson.GetComponent<personAttributes>().health = person.health;
                            tmpPerson.GetComponent<personAttributes>().type = person.type;
                            break;
                        }
                    case 6:
                        {
                            //bowarrow
                            GameObject tmpPerson = Instantiate(bowarrow, placePosition, Quaternion.Euler(eulerRotation));
                            tmpPerson.GetComponent<personAttributes>().health = person.health;
                            tmpPerson.GetComponent<personAttributes>().type = person.type;
                            break;
                        }
                    case 7:
                        {
                            //scout
                            GameObject tmpPerson = Instantiate(scout, placePosition, Quaternion.Euler(eulerRotation));
                            tmpPerson.GetComponent<personAttributes>().health = person.health;
                            tmpPerson.GetComponent<personAttributes>().type = person.type;
                            break;
                        }
                    case 8:
                        {
                            //scout
                            GameObject tmpPerson = Instantiate(knight, placePosition, Quaternion.Euler(eulerRotation));
                            tmpPerson.GetComponent<personAttributes>().health = person.health;
                            tmpPerson.GetComponent<personAttributes>().type = person.type;
                            break;
                        }
                    default:
                        {
                            //GameObject tmpPerson = Instantiate(civilianM, placePosition, Quaternion.Euler(eulerRotation));
                            //tmpPerson.GetComponent<personAttributes>().health = person.health;
                            //tmpPerson.GetComponent<personAttributes>().type = 0;
                            break;
                        }
                }
                
                //gameStatistic.GS.persons.Add(tmpPerson);//already added in person start()
            }

            //place enemys
            gameStatistic.GS.removeAllEnemys();
            foreach (EnemyData enemy in enemyData)
            {
                placePosition = new Vector3(enemy.posx, enemy.posy, enemy.posz);
                eulerRotation = new Vector3(enemy.rotx, enemy.roty, enemy.rotz);
                switch (enemy.type)
                {
                    case 1:
                        {
                            //enemy1
                            GameObject tmpPerson = Instantiate(enemy1, placePosition, Quaternion.Euler(eulerRotation));
                            tmpPerson.GetComponent<enemyAttributes>().health = enemy.health;
                            tmpPerson.GetComponent<enemyAttributes>().type = enemy.type;
                            break;
                        }
                    case 2:
                        {
                            //enemy2
                            GameObject tmpPerson = Instantiate(enemy2, placePosition, Quaternion.Euler(eulerRotation));
                            tmpPerson.GetComponent<enemyAttributes>().health = enemy.health;
                            tmpPerson.GetComponent<enemyAttributes>().type = enemy.type;
                            break;
                        }
                    case 3:
                        {
                            //enemy3
                            GameObject tmpPerson = Instantiate(enemy3, placePosition, Quaternion.Euler(eulerRotation));
                            tmpPerson.GetComponent<enemyAttributes>().health = enemy.health;
                            tmpPerson.GetComponent<enemyAttributes>().type = enemy.type;
                            break;
                        }
                }
            }

            return true;
        }
        else
        {
            Debug.Log("Couldn't load file: " + Application.dataPath + "/saves/" + globalVariables.loadedSaveName + "/map.save");
            return false;
        }
        
    }


    [Serializable]
    public class BuildingData
    {
        public string name;
        public int type;
        public int subtype;
        public int width;
        public int height;
        public float posx;
        public float posy;
        public float posz;
        public float rotx;
        public float roty;
        public float rotz;
    }

    [Serializable]
    public class PersonData
    {
        public string name;
        public float health;
        public int type;
        public float posx;
        public float posy;
        public float posz;
        public float rotx;
        public float roty;
        public float rotz;
    }

    [Serializable]
    public class EnemyData
    {
        public float health;
        public int type;
        public float posx;
        public float posy;
        public float posz;
        public float rotx;
        public float roty;
        public float rotz;
    }

    [Serializable]
    public class MapData
    {
        public int collectedWood;
        public int collectedStone;
        public int collectedIron;
        public int collectedFood;
        public int availableBeds;
        public int size;

        public int[] mapgrid;
        public float[] vertexes;
        public int[] tris;
    }

    public class JsonVector
    {
        public float x;
        public float y;
        public float z;
    }

    //https://stackoverflow.com/questions/36239705/serialize-and-deserialize-json-and-json-array-in-unity
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}
