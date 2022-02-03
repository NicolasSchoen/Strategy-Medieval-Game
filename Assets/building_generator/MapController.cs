using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public int mapSize = 100;
    MapGrid mapgrid;
    public MeshGenerator mapMesh;

    public Material grassMaterial;
    public Material gridMaterial;

    public int nmbrFortress = 1;
    public int nmbrTree = 1000;
    public int nmbrRock = 100;
    public int nmbrMountain = 10;

    public GameObject treeObject;
    public GameObject treeObject2;
    public GameObject stoneObject;
    public GameObject stoneObject2;
    public GameObject ironObject;
    public GameObject fortressObject;
    public GameObject enemyCamp;
    public GameObject baseCamp;

    public GameObject beach;

    public static MapController MC;

    // Start is called before the first frame update
    void Start()
    {
        //placedObjects = new List<GameObject>();
        //StartCoroutine(loadWorld());//runs in the background in a new thread
        MC = this;
        loadWorld();
        

        if (globalVariables.loadSaveGame) GetComponent<savegameController>().loadFromFile();
    }

    private void removeObjects()
    {
        foreach (GameObject placedObject in gameStatistic.GS.placedObjects)
        {
            Destroy(placedObject);
        }
            

        gameStatistic.GS.placedObjects.Clear();
        gameStatistic.GS.resetValues();
    }

    public void createNewWorld()
    {
        loadWorld();
        beach.GetComponent<beachMesh>().recreateBeach();
    }

    public void loadWorld()
    {
        mapSize = globalVariables.mapSize;
        removeObjects();
        mapMesh.setSize(mapSize);
        mapMesh.createMap(mapgrid);
        mapgrid = new MapGrid(mapSize, nmbrFortress, nmbrTree, nmbrRock, nmbrMountain);
        //mapMesh.setSize(mapSize);
        //mapMesh.createMap(mapgrid);
        placeObjectsFromMap();
        //yield return null;
    }

    public void generateBuildPlace(object[] tmpstorage,bool dontRequireChecking=false)
    {
        generateBuildPlace((Vector3)tmpstorage[0],(int)tmpstorage[1],(int)tmpstorage[2],dontRequireChecking);
    }

    public void generateBuildPlace(Vector3 position, int length, int width, bool dontRequireChecking=false)
    {
        if (mapgrid.checkMapGrid(position, length, width) || dontRequireChecking)
        {
            mapMesh.generateBuildPlaceOnMesh(position, length, width);
            beach.GetComponent<beachMesh>().recreateBeach();
        }
    }

    public int[,] getMap()
    {
        return mapgrid.getMapGrid();
    }

    public int[] getTris()
    {
        return mapMesh.triangles;
    }

    public float[] getVertexes()
    {
        return mapMesh.getVertexes();
    }

    public bool loadMap(int newSize,int[] newSerializedMapGrid,int[] tris, float[] vertices)
    {
        removeObjects();
        mapSize = newSize;
        int[,] newMapGrid = deSerializeMapGrid(newSerializedMapGrid);
        mapgrid.loadMapGrid(newSize,newMapGrid);
        mapMesh.loadMapMesh(mapSize, mapgrid, tris, vertices);
        return true;
    }

    private int[,] deSerializeMapGrid(int[] serializedMapGrid)
    {
        int[,] tempMapGrid = new int[mapSize, mapSize];
        int counter = 0;

        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                tempMapGrid[x, y] = serializedMapGrid[counter];
                counter++;
            }
        }
        return tempMapGrid;
    }

    public int[] getSerializedMapGrid()
    {
        int[] returnMapGrid = new int[mapSize * mapSize];
        int count = 0;
        foreach (int value in mapgrid.getMapGrid())
        {
            returnMapGrid[count] = value;
            count++;
        }
        return returnMapGrid;
    }

    public void changeTerrain(object[] tmpstorage) //call: changeTerrain([point,difference,vertexAmount])
    {
        Debug.Log("Request to change the terrain");
        if(mapgrid.checkMapGrid((Vector3) tmpstorage[0], (int) tmpstorage[2], (int)tmpstorage[2]))
        {
            mapMesh.changeTerrainOnMesh(tmpstorage);
            beach.GetComponent<beachMesh>().recreateBeach();
        }
    }

    public bool changeMapGrid(int x, int z, int val) //posVector, length, width, value
    {
        return mapgrid.setValue(x, z, val);
    }

    public bool checkMap(Vector3 position, int width, int length)
    {
        return mapgrid.checkMapGrid(position, width, length);
    }

    public void placeObject(object[] tmpstorage)
    {
        placeObject((Vector3)tmpstorage[0],(int)tmpstorage[1],(int)tmpstorage[2],(int)tmpstorage[3],(GameObject)tmpstorage[4],(Quaternion)tmpstorage[5]);
    }

    public void changeMapGridDirectly(Vector3 position, int width, int length, int value)
    {
        Vector3 originPos = new Vector3(position.x - ((float)width/2),0,position.z - ((float)length/2));
        //Debug.Log("Building position: " + position.ToString());
        //Debug.Log("Destroying Building at: " + originPos.ToString());
        mapgrid.changeMapGrid(originPos, width, length, value);
    }

    public GameObject placeObject(Vector3 position, int length, int width, int type, GameObject opject, Quaternion rotation)
    {
        if (!opject.GetComponent<ModelAttributes>().checkRessources()) return null;
        if (mapgrid.changeMapGrid(position, width, length, type))
        {
            opject.GetComponent<ModelAttributes>().useRessources();

            //flatten terrain if building requires buildplace
            Debug.Log("requires Buildplace: " + opject.GetComponent<ModelAttributes>().getRequireBuildplace().ToString());
            if (opject.GetComponent<ModelAttributes>().getRequireBuildplace())
            {
                generateBuildPlace(position,width,length, true);
            }
            float newx = position.x += ((float)width / 2);
            float newy = mapMesh.getBlockHeight((int)position.x, (int)position.z);//placePosition.y;
            float newz = position.z += ((float)length / 2);
            position = new Vector3(newx, newy, newz);
            GameObject placedObject = Instantiate(opject, position, rotation);
            gameStatistic.GS.placedObjects.Add(placedObject);
            placedObject.GetComponent<ModelAttributes>().placeObject();
            return placedObject;
        }
        return null;
    }

    public float getHeight(int x, int z)
    {
        return mapMesh.getBlockHeight(x, z);
    }

    public float getVertexHeight(int x, int z)
    {
        return mapMesh.getVertexHeight(x, z);
    }

    public float getHeight(float x, float z)
    {
        if (x < 0 || x >= mapSize || z < 0 || z > mapSize) return -1;
        return mapMesh.getBlockHeight((int)x, (int)z);
    }

    public void setMeshTextureGrid(bool value)
    {
        Debug.Log("Set Grid texture: " + value.ToString());
        if (value)
        {
            Material[] newmaterial = {grassMaterial,gridMaterial };
            GetComponent<Renderer>().materials = newmaterial;
        }
        else
        {
            Material[] newmaterial = { grassMaterial};
            GetComponent<Renderer>().materials = newmaterial;
        }
    }

    public void placeObjectsFromMap()
    {
        for (int z = 0; z < mapSize; z++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                if(mapgrid.getValue(x,z) == 1)  //if tree
                {
                    if(Random.Range(0,2) >= 1) gameStatistic.GS.placedObjects.Add(Instantiate(treeObject, new Vector3(x + .5f, mapMesh.getBlockHeight(x, z), z + .5f), Quaternion.Euler(0, Random.Range(0, 360), 0)));
                    else gameStatistic.GS.placedObjects.Add(Instantiate(treeObject2,new Vector3(x+.5f, mapMesh.getBlockHeight(x,z) ,z+.5f), Quaternion.Euler(0, Random.Range(0, 360), 0)));
                }

                if (mapgrid.getValue(x, z) == 4)  //if rock
                {
                    if (Random.Range(0, 2) >= 1) gameStatistic.GS.placedObjects.Add(Instantiate(stoneObject, new Vector3(x+.5f, mapMesh.getBlockHeight(x, z), z+.5f), Quaternion.identity));
                    else gameStatistic.GS.placedObjects.Add(Instantiate(stoneObject2, new Vector3(x + .5f, mapMesh.getBlockHeight(x, z), z + .5f), Quaternion.identity));
                }

                if (mapgrid.getValue(x, z) == 3)  //if iron
                {
                    mapMesh.generateBuildPlaceOnMesh(new Vector3(x,0,z), 4, 4);
                    gameStatistic.GS.placedObjects.Add(Instantiate(ironObject, new Vector3(x+2, mapMesh.getBlockHeight(x, z), z+2), Quaternion.identity));
                }

                if (mapgrid.getValue(x, z) == 12)  //if fortress
                {
                    mapMesh.generateBuildPlaceOnMesh(new Vector3(x, 0, z), 5, 5);
                    gameStatistic.GS.placedObjects.Add(Instantiate(fortressObject, new Vector3(x + 2.5f, mapMesh.getBlockHeight(x, z), z + 2.5f), Quaternion.identity));
                }

                if (mapgrid.getValue(x, z) == 13)  //if enemy camp
                {
                    mapMesh.generateBuildPlaceOnMesh(new Vector3(x, 0, z), 3, 3);
                    gameStatistic.GS.placedObjects.Add(Instantiate(enemyCamp, new Vector3(x + 1.5f, mapMesh.getBlockHeight(x, z), z + 1.5f), Quaternion.identity));
                }

                if (mapgrid.getValue(x, z) == 50)  //if camp
                {
                    GameObject playercamp;
                    mapMesh.generateBuildPlaceOnMesh(new Vector3(x, 0, z), 3, 3);
                    playercamp = Instantiate(baseCamp, new Vector3(x + 1.5f, mapMesh.getBlockHeight(x, z), z + 1.5f), Quaternion.identity);
                    gameStatistic.GS.placedObjects.Add(playercamp);

                    playercamp.GetComponent<ModelAttributes>().placeObject();
                }
            }
        }
    }
}
