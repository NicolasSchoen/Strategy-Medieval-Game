using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beachMesh : MonoBehaviour
{
    Mesh mesh;
    MeshCollider meshc;

    public float xOff = .05f;
    public float zOff = .05f;
    public float strength = 10;
    public float width = 20;
    public float sealevel = -1;

    Vector3[] vertices;

    Vector2[] uvs;

    public int[] triangles;

    int size = 100;

    // Start is called before the first frame update
    void Start()
    {
        size = globalVariables.mapSize;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        if (meshc == null) meshc = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;

        CreateShape();
        UpdateMesh();
    }

    public void setSize(int newsize)
    {
        size = newsize;
    }

    public void recreateBeach()
    {
        size = globalVariables.mapSize;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        if (meshc == null) meshc = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;

        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        vertices = new Vector3[(size + 1) * 2 * 4 + 16];//+ 4*4 for edges
        uvs = new Vector2[vertices.Length];

        triangles = new int[(6 * size) * 4 + 24];//+6*4 for edges

        int i = 0;
        //left beach
        for (int y = 0; y <= size; y++)
        {
            for (int x = 0; x <= 1; x++)
            {
                if(x > 0) vertices[i] = new Vector3(0, MapController.MC.getVertexHeight(0,y), y);
                else vertices[i] = new Vector3(-width, sealevel, y);

                uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
                i++;
            }
        }

        //right beach
        for (int y = 0; y <= size; y++)
        {
            for (int x = 0; x <= 1; x++)
            {
                if(x>0) vertices[i] = new Vector3(size + width, sealevel, y);
                else vertices[i] = new Vector3(size, MapController.MC.getVertexHeight(size, y), y);
                uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
                i++;
            }
        }

        //down beach
        for (int x = 0; x <= size; x++)
        {
            for (int y = 0; y <= 1; y++)
            {
                if(y>0) vertices[i] = new Vector3(x, sealevel, - width * y);
                else vertices[i] = new Vector3(x, MapController.MC.getVertexHeight(x, 0), y);
                uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
                i++;
            }
        }

        //up beach
        for (int x = 0; x <= size; x++)
        {
            for (int y = 0; y <= 1; y++)
            {
                if(y>0) vertices[i] = new Vector3(x, MapController.MC.getVertexHeight(x, size), size);
                else vertices[i] = new Vector3(x , sealevel, y+size+ width);
                uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
                i++;
            }
        }
        //edge 00
        vertices[i] = new Vector3(-width, sealevel, -width);
        uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        i++;
        vertices[i] = new Vector3(0, sealevel, -width);
        uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        i++;
        vertices[i] = new Vector3(-width, sealevel, 0);
        uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        i++;
        vertices[i] = new Vector3(0, MapController.MC.getVertexHeight(0, 0), 0);
        uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        i++;
        //edge 10
        vertices[i] = new Vector3(size, sealevel, -width);
        uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        i++;
        vertices[i] = new Vector3(size+ width, sealevel, -width);
        uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        i++;
        vertices[i] = new Vector3(size, MapController.MC.getVertexHeight(size, 0), 0);
        uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        i++;
        vertices[i] = new Vector3(size+ width, sealevel, 0);
        uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        i++;
        //edge 01
        vertices[i] = new Vector3(-width, sealevel, size);
        uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        i++;
        vertices[i] = new Vector3(0, MapController.MC.getVertexHeight(0, size), size);
        uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        i++;
        vertices[i] = new Vector3(-width, sealevel, size+ width);
        uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        i++;
        vertices[i] = new Vector3(0, sealevel, size+ width);
        uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        i++;
        //edge 11
        vertices[i] = new Vector3(size, MapController.MC.getVertexHeight(size, size), size);
        uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        i++;
        vertices[i] = new Vector3(size+ width, sealevel, size);
        uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        i++;
        vertices[i] = new Vector3(size, sealevel, size+ width);
        uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        i++;
        vertices[i] = new Vector3(size+ width, sealevel, size+ width);
        uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        i++;

        int vert = 0;
        int tris = 0;

        //left
        for (int z = 0; z < size; z++)
        {
            triangles[tris] = vert;
            triangles[tris + 1] = vert + 2;
            triangles[tris + 2] = vert + 1;
            triangles[tris + 3] = vert + 1;
            triangles[tris + 4] = vert + 2;
            triangles[tris + 5] = vert + 3;

            vert+=2;
            tris += 6;
        }
        vert += 2;
        //right
        for (int z = 0; z < size; z++)
        {
            triangles[tris] = vert;
            triangles[tris + 1] = vert + 2;
            triangles[tris + 2] = vert + 1;
            triangles[tris + 3] = vert + 1;
            triangles[tris + 4] = vert + 2;
            triangles[tris + 5] = vert + 3;

            vert+=2;
            tris += 6;
        }
        vert += 2;
        //down
        for (int z = 0; z < size; z++)
        {
            triangles[tris] = vert;
            triangles[tris + 1] = vert + 2;
            triangles[tris + 2] = vert + 1;
            triangles[tris + 3] = vert + 1;
            triangles[tris + 4] = vert + 2;
            triangles[tris + 5] = vert + 3;

            vert+=2;
            tris += 6;
        }
        vert += 2;
        //up
        for (int z = 0; z < size; z++)
        {
            triangles[tris] = vert;
            triangles[tris + 1] = vert + 2;
            triangles[tris + 2] = vert + 1;
            triangles[tris + 3] = vert + 1;
            triangles[tris + 4] = vert + 2;
            triangles[tris + 5] = vert + 3;

            vert+=2;
            tris += 6;
        }
        vert += 2;
        //edges
        for (int j = 0; j < 4; j++)
        {
            if(j == 0 || j == 3)
            {
                triangles[tris] = vert;
                triangles[tris + 1] = vert + 2;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + 2;
                triangles[tris + 5] = vert + 3;
            }
            else
            {
                triangles[tris] = vert;
                triangles[tris + 1] = vert + 2;
                triangles[tris + 2] = vert + 3;
                triangles[tris + 3] = vert;
                triangles[tris + 4] = vert + 3;
                triangles[tris + 5] = vert + 1;
            }

            vert += 4;
            tris += 6;
        }
    }

    public float[] getVertexes()
    {
        float[] returnValues = new float[vertices.Length * 3];
        int counter = 0;
        for (int i = 0; i < vertices.Length; i++)
        {
            returnValues[counter] = vertices[i].x;
            returnValues[counter + 1] = vertices[i].y;
            returnValues[counter + 2] = vertices[i].z;
            counter += 3;
        }
        return returnValues;
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
