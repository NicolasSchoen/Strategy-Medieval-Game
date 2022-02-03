using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    MeshCollider meshc;

    public float xOff = .05f;
    public float zOff = .05f;
    public float strength = 10;
    public float height = 2;
    public float sealevel = -1;


    Vector3[] vertices;

    Vector2[] uvs;

    public int[] triangles;

    MapGrid mapGrid;

    int size = 100;

    // Start is called before the first frame update
    void Start()
    {
        /*mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        meshc = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;

        CreateShape();
        UpdateMesh();
        mapGrid = new MapGrid(size);*/
    }

    public void setSize(int newsize)
    {
        size = newsize;
    }

    public void createMap(MapGrid mgrid)
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        if (meshc == null) meshc = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;

        CreateShape();
        UpdateMesh();
        mapGrid = mgrid;
    }

    float generateHeight(int x, int z, float randoffset)    //flater world
    {
        float h = 0f;
        h = Mathf.PerlinNoise(x * xOff + randoffset, z * zOff + randoffset) * strength;
        h = Mathf.Sin(h) * height;
        if (h < sealevel) h = sealevel;
        return h;
    }

    float generateHeight2(int x, int z, float randoffset)   //higher mountains
    {
        float h = Mathf.PerlinNoise(x * xOff + randoffset, z * zOff + randoffset) * strength;
        float h2 = Mathf.PerlinNoise(x * (xOff / 2) + randoffset, z * (zOff / 2) + randoffset) * strength;
        h = Mathf.Sin(h) * height;
        h = -Mathf.Abs(h) + h2 * 2;
        if (h < sealevel) h = sealevel;
        return h;
    }

    float generateHeight3(int x, int z, float randoffset)   //more interesting world
    {
        float h = Mathf.PerlinNoise(x * xOff + randoffset, z * zOff + randoffset) * strength;
        float h2 = Mathf.PerlinNoise(x * (xOff / 2) + randoffset, z * (zOff / 2) + randoffset) * strength;
        h = Mathf.Sin(h) * height;
        h = -Mathf.Abs(h) + h2 / 2;
        if (h < sealevel) h = sealevel;
        return h;
    }

    void CreateShape()
    {
        float water = sealevel;

        vertices = new Vector3[(size + 1) * (size + 1)];
        uvs = new Vector2[vertices.Length];

        triangles = new int[(size * size) * 6];

        int i = 0;
        float randoffset = Random.Range(1, 100);
        for (int y = 0; y <= size; y++)
        {
            for (int x = 0; x <= size; x++)
            {
                //water = (x*x)+(size / 2);
                vertices[i] = new Vector3(x, generateHeight3(x, y, randoffset), y);
                uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
                i++;
            }
        }

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                triangles[tris] = vert;
                triangles[tris + 1] = vert + size + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + size + 1;
                triangles[tris + 5] = vert + size + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        /*vertices = new Vector3[]
        {
            new Vector3(0,h,0),
            new Vector3(0,h,1),
            new Vector3(1,h,0),
            new Vector3(1,h,1)
        };

        triangles = new int[]
        {
            0,1,2,
            1,3,2
        };*/
    }

    public void generateBuildPlaceOnMesh(Vector3 clickPoint, int length, int width)
    {
        int posx = (int)clickPoint.x;
        int posz = (int)clickPoint.z;
        Debug.Log("Changing terrain at " + posx.ToString() + " | " + posz.ToString());

        int selectedVertices = 0;
        float avgHeight = 0;
        for (int x = 0; x <= length; x++)
        {
            for (int z = 0; z <= width; z++)
            {
                avgHeight += vertices[((posz + z) * (size + 1)) + (posx + x)].y;
                selectedVertices++;
            }
        }
        avgHeight /= selectedVertices;


        for (int x = 0; x <= length; x++)
        {
            for (int z = 0; z <= width; z++)
            {
                vertices[((posz + z) * (size + 1)) + (posx + x)].y = avgHeight;
            }
        }


        Debug.Log("Changing terrain at " + posx.ToString() + " | " + posz.ToString() + " Number of changed Vertices: " + selectedVertices.ToString() + " avg: " + avgHeight.ToString());
        UpdateMesh();
    }

    public float[] getVertexes()
    {
        float[] returnValues = new float[vertices.Length * 3];
        int counter = 0;
        for (int i = 0; i < vertices.Length; i++)
        {
            returnValues[counter] = vertices[i].x;
            returnValues[counter+1] = vertices[i].y;
            returnValues[counter+2] = vertices[i].z;
            counter+= 3;
        }
        return returnValues;
    }

    public bool loadMapMesh(int size,MapGrid mgrid,int[] mTris, float[] mVertexes)
    {
        int counter = 0;
        setSize(size);

        CreateShape();//should fix BUG

        for (int i = 0; i < mVertexes.Length/3; i++)
        {
            vertices[i] = new Vector3(mVertexes[counter], mVertexes[counter+1], mVertexes[counter+2]);
            counter += 3;
        }
        triangles = mTris;

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        if(meshc == null)meshc = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        UpdateMesh();
        mapGrid = mgrid;
        return true;
    }

    public void changeTerrainOnMesh(object[] tmpstorage) //call: changeTerrain([point,difference,vertexAmount])
    {
        Vector3 clickPoint = (Vector3)tmpstorage[0];
        int posx;// = Mathf.RoundToInt(clickPoint.x);
        int posz;// = Mathf.RoundToInt(clickPoint.z);
        float heightDiff = (float)tmpstorage[1];
        int length = (int)tmpstorage[2];

        //make click change the block if selected more than one vertex to change (length > 0)
        if (length <= 0)
        {
            posx = Mathf.RoundToInt(clickPoint.x);
            posz = Mathf.RoundToInt(clickPoint.z);
        }
        else
        {
            posx = (int)clickPoint.x;
            posz = (int)clickPoint.z;
        }

        for (int x = 0; x <= length; x++)
        {
            for (int z = 0; z <= length; z++)
            {
                if (heightDiff < 0 && vertices[((posz + z) * (size + 1)) + (posx + x)].y < sealevel) continue;
                vertices[((posz + z) * (size + 1)) + (posx + x)].y += heightDiff;
            }
        }


        Debug.Log("Changing terrain at " + posx.ToString() + " | " + posz.ToString());
        UpdateMesh();
    }

    public bool changeMapGrid(Vector3 clickPoint, int length, int width, int value) //posVector, length, width, value
    {
        int posx = (int)clickPoint.x;
        int posz = (int)clickPoint.z;
        bool retval = true;

        int xfind = posx;  //used to store already changed values
        int zfind = posz;

        //workaround: first search if free, then place if free
        for (zfind = posz; zfind < length + posz; zfind++)
        {
            if (!retval) break;
            for (xfind = posx; xfind < width + posx; xfind++)
            {
                if (mapGrid.getValue(xfind, zfind) > 0) //if already full
                {
                    retval = false;
                }
            }
        }

        if (retval || value == 0)
        {
            for (zfind = posz; zfind < length + posz; zfind++)
            {
                for (xfind = posx; xfind < width + posx; xfind++)
                {
                    //Debug.Log("Changing " + xfind.ToString() + " | " + zfind.ToString());
                    //Debug.Log("1: " + mapGrid.getValue(xfind, zfind).ToString());
                    mapGrid.setValue(xfind, zfind, value);
                    //Debug.Log("2: " + mapGrid.getValue(xfind,zfind).ToString());
                }
            }
        }

        /*if (!retval)        //if field couldn't be changed, eg object was blocking, reset all placed fields to 0
        {
            for (int z = posz; z <= zfind; z++)
            {
                for (int x = posx; x <= xfind; x++)
                {
                    mapGrid.setValue(x, z, 0);
                }
            }
        }*/
        return retval;
    }

    public float getBlockHeight(int posx, int posz)
    {
        int selectedVertices = 0;
        float avgHeight = 0;
        for (int x = 0; x <= 1; x++)
        {
            for (int z = 0; z <= 1; z++)
            {
                avgHeight += vertices[((posz + z) * (size + 1)) + (posx + x)].y;
                selectedVertices++;
            }
        }
        avgHeight /= selectedVertices;
        return avgHeight;
    }

    public float getVertexHeight(int posx, int posz)
    {
        return vertices[((posz) * (size + 1)) + (posx)].y;
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.uv = uvs;

        mesh.RecalculateNormals();

        meshc.sharedMesh = null;
        meshc.sharedMesh = mesh;
    }
}
