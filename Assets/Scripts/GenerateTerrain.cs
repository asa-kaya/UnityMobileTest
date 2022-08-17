using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider), typeof(MeshRenderer))]
public class GenerateTerrain : MonoBehaviour
{
    public int xSize;
    public int zSize;

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    float noiseScale = 0f;
    float heightScale = 3f;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;

        transform.position = new Vector3(-xSize/2, 0, -zSize/2);

        GenerateShape();
        GenerateMesh();
    }

    void GenerateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        
        for (int i = 0, x = 0; x <= xSize; x++)
        {
            for (int z = 0; z <= zSize; z++)
            {
                float y = Mathf.PerlinNoise(x * noiseScale, z * noiseScale) * heightScale;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        for (int i = 0, x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                int v = x + (x * zSize) + z;

                triangles[i] = v;
                triangles[i+1] = v + 1;
                triangles[i+2] = v + zSize + 1;
                triangles[i+3] = v + 1;
                triangles[i+4] = v + zSize + 2;
                triangles[i+5] = v + zSize + 1;
                i += 6;
            }
        }
    }

    void GenerateMesh()
    {
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    void OnDrawGizmos()
    {
        if (vertices == null)
            return;
        
        for (int i = 0; i < 10; i++)
            Gizmos.DrawSphere(vertices[i], .1f);
    }
}
