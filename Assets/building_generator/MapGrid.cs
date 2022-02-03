using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid
{
    private int[,] map; //valid values:
    private int size;
    private float treeLevel = .2f;

    private int nmbrFortress;
    private int nmbrEnemycamp;
    private int nmbrTree;
    private int nmbrRock;
    private int nmbrMountain;
    /*
     * 0: none
     * 1: tree
     * 2: field
     * 3: iron Mountain
     * 4: stone
     * 9: placeholder for larger objects
     * 10: simple_tower
     * 11: stone_tower
     * 12: fortress
     * 13: enemy camp
     * 14: castle
     * 20: wood_house
     * 21: stone_house
     * 22: small house
     * 23: big house
     * 24: forestry
     * 25: farm
     * 26: mine iron
     * 27: mine stone
     * 28: food storage
     * 29: resource storage
     * 30: wood fence |
     * 30: wood fence -
     * 31: wood fence edge right
     * 32: wood fence edge down
     * 33: wood fence edge left
     * 34: wood fence edge up
     * 35: stone fence |
     * 36: stone fence edge
     * 50: camp
     * 60: catapult
     * 61: street lamp
     * 62: well
     * 70: church
     * 72: watch tower
     * 73: large tower
     * 75: Blacksmith
     * 76: Butcher
     * 77: city Wall
     * 78: wall gate
     * 79: shooting range
     */

    public MapGrid(int size, int f, int t, int r, int m)
    {
        this.size = size;
        nmbrFortress = f;
        nmbrEnemycamp = 2 * f;
        nmbrTree = t;
        nmbrRock = r;
        nmbrMountain = m;
        map = new int[size,size];
        initialize();
    }

    private void initialize()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                map[i, j] = 0;
            }
        }
        placeBaseCamp();    //place camp in the centre
        if (!globalVariables.isPeaceful)
        {
            placeFortress(nmbrFortress);
            placeEnemyCamp(nmbrEnemycamp);
        }
        placeIron(nmbrMountain);
        placeRocks(nmbrRock);
        placeTrees();
        
    }

    private void placeBaseCamp()
    {
        int x = size/2 - 2;
        int y = size/2 - 2;
        if (checkMapGrid(new Vector3(x, 0, y), 3, 3))
        {
            changeMapGrid(new Vector3(x, 0, y), 3, 3, 50);   //place Camp (3)
        }
    }

    private void placeFortress(int amount)
    {
        int placedFortress = 0;
        int x;
        int y;
        while (placedFortress < amount)
        {
            x = Random.Range(0, size - 4);
            y = Random.Range(0, size - 4);
            if (checkNearbyBasecamp(x, y)) continue;
            if (checkMapGrid(new Vector3(x, 0, y), 5, 5))
            {
                changeMapGrid(new Vector3(x, 0, y), 5, 5, 12);   //place Iron (3)
                placedFortress++;
            }
        }
    }

    private void placeEnemyCamp(int amount)
    {
        int placedEnemyCamp = 0;
        int x;
        int y;
        while (placedEnemyCamp < amount)
        {
            x = Random.Range(0, size - 2);
            y = Random.Range(0, size - 2);
            if (checkNearbyBasecamp(x, y)) continue;
            if (checkMapGrid(new Vector3(x, 0, y), 3, 3))
            {
                changeMapGrid(new Vector3(x, 0, y), 3, 3, 13);   //place Enemy Camp
                placedEnemyCamp++;
            }
        }
    }

    private bool checkNearbyBasecamp(int x, int y)
    {
        if (Mathf.Abs(size/2 -x) < 15  || Mathf.Abs(size / 2 - y) < 15) return true;
        return false;
    }

    private void placeIron(int amount)
    {
        amount = (int)(amount * (size/100f));
        amount = Random.Range(amount / 5, amount);
        int placedMountain = 0;
        int x;
        int y;
        int errorcount = 0;
        while (placedMountain < amount)
        {
            x = Random.Range(0, size-3);
            y = Random.Range(0, size-3);
            if (checkMapGrid(new Vector3(x,0,y),4,4) && MapController.MC.getHeight(x, y) >= treeLevel)
            {
                changeMapGrid(new Vector3(x, 0, y), 4, 4, 3);   //place Iron (3)
                placedMountain++;
            }
            else
            {
                errorcount++;
            }
            if (errorcount > 10 * amount) break;
        }
    }

    private void placeTrees()
    {
        int plantedTrees = 0;
        int x;
        int y;
        int errorcount = 0;
        int amount = Random.Range(size * (size / 10), size * (size/5));
        while(plantedTrees < amount)
        {
            x = Random.Range(0, size);
            y = Random.Range(0, size);
            if(map[x,y] == 0 && MapController.MC.getHeight(x, y) >= treeLevel)
            {
                map[x, y] = 1;  //plant tree
                plantedTrees++;
            }
            else
            {
                errorcount++;
            }
            if (errorcount > 10 * amount) break;
        }
        /*int nmbrForests = Random.Range(size/16,size/2);
        for (int i = 0; i < nmbrForests; i++)
        {
            placeForest(Random.Range(0, size), Random.Range(0, size), Random.Range(size/12, size/3));
        }*/
    }

    private void placeForest(int posx, int posz, int forestSize)
    {
        int nmbrTrees = forestSize * forestSize/Random.Range(1,10);
        int plantedTrees = 0;
        int x;
        int y;
        int errorcount = 0;
        int maxX = size;
        int maxZ = size;
        if (posx + forestSize < size) maxX = posx + forestSize;
        if (posz + forestSize < size) maxZ = posz + forestSize;

        for (int i = 0; i < nmbrTrees; i++)
        {
            x = Random.Range(posx, maxX);
            y = Random.Range(posz, maxZ);
            if (map[x, y] == 0 && MapController.MC.getHeight(x, y) >= treeLevel)
            {
                map[x, y] = 1;  //plant tree
                plantedTrees++;
            }
            else
            {
                errorcount++;
            }
            if (errorcount > 10 * nmbrTrees) break;
        }
    }

    private void placeRocks(int amount)
    {
        amount = (int)(amount * (size / 100f));
        amount = Random.Range(amount/2, amount*3);
        int placedRocks = 0;
        int x;
        int y;
        int errorcount = 0;
        while (placedRocks < amount)
        {
            x = Random.Range(0, size);
            y = Random.Range(0, size);
            if (map[x, y] == 0 && MapController.MC.getHeight(x, y) >= treeLevel)
            {
                map[x, y] = 4;  //place rock
                placedRocks++;
            }
            else
            {
                errorcount++;
            }
            if (errorcount > 10 * amount) break;
        }
    }

    public bool setValue(int x, int y, int value)
    {
        //Debug.Log("Inside setValue: x=" + x.ToString() + " y=" + y.ToString() + " value=" + value.ToString());
        if((value > 0) && (map[x,y] == 0))    //if there is no block placed, place new block, otherwise block must be destroyed first
        {
            map[x, y] = value;
            return true;
        }
        if(value == 0) map[x, y] = value;

        return false;
    }

    public int getValue(int x, int y)
    {
        return map[x, y];
    }

    public bool checkMapGrid(Vector3 clickPoint, int width, int length) //posVector, length, width, value
    {
        int posx = (int)clickPoint.x;
        int posz = (int)clickPoint.z;
        bool retval = true;

        if ((posx + width - 1) >= size || posx < 0) return false;
        if ((posz + length - 1) >= size || posz < 0) return false;

        int xfind = posx;  //used to store already changed values
        int zfind = posz;

        //workaround: first search if free, then place if free
        for (zfind = posz; zfind < length + posz; zfind++)
        {
            if (!retval) break;
            for (xfind = posx; xfind < width + posx; xfind++)
            {
                if (getValue(xfind, zfind) > 0) //if already full
                {
                    retval = false;
                }
            }
        }
        return retval;
    }

    public bool loadMapGrid(int newSize,int[,] newGrid)
    {
        size = newSize;
        map = newGrid;
        return true;
    }

    public bool changeMapGrid(Vector3 clickPoint, int width, int length, int value) //posVector, length, width, value
    {
        int posx = (int)clickPoint.x;
        int posz = (int)clickPoint.z;
        bool retval = true;

        int xfind = posx;  //used to store already changed values
        int zfind = posz;

        //workaround: first search if free, then place if free
        retval = checkMapGrid(clickPoint, width, length);

        if (retval || value == 0)
        {
            int count = 0;
            for (zfind = posz; zfind < length + posz; zfind++)
            {
                for (xfind = posx; xfind < width + posx; xfind++)
                {
                    if(count == 0 || value == 0)
                    {
                        setValue(xfind, zfind, value);
                        count++;
                    }
                    else
                    {
                        setValue(xfind, zfind, 9);
                    }
                }
            }
        }
        return retval;
    }

    public int[,] getMapGrid()
    {
        return map;
    }
}
