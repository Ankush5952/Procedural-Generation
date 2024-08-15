using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    public int xSize = 20, zSize = 20;
    public Gradient gradient;
    public Octave[] octaves;
    public float noiseStrength = 1;

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    Color[] colors;
    float minTerrainHeight =  int.MinValue;
    float maxTerrainHeight =  int.MaxValue;

    void Start()
    {
        mesh = new Mesh();

        GetComponent<MeshFilter>().mesh = mesh;

    }

    void Update()
    {
        CreateShape();
        UpdateMesh();
    }

    void OnValidate()
    {

        

        if(xSize <= 0) xSize = 1;
        if(zSize <= 0) zSize = 1;
        if(noiseStrength <= 0) noiseStrength = 0.1f;
    }

    void CreateShape()
    {
        UpdateVertices();
        UpdateTriangles();
        UpdateColors();
    } 

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
    }

    void UpdateVertices()
    {
        minTerrainHeight = int.MaxValue;
        maxTerrainHeight = int.MinValue;
        vertices = new Vector3[ (xSize + 1) * (zSize + 1) ];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = 0;
                for (int k = 0; k < octaves.Length; k++)
                {
                    y += octaves[k].amplitude * Mathf.PerlinNoise(x*octaves[k].frequency, z*octaves[k].frequency) * noiseStrength;
                }
                vertices[i++] = new Vector3(x, y, z);

                if(minTerrainHeight > y) 
                { minTerrainHeight = y; }
                if(maxTerrainHeight < y) 
                { maxTerrainHeight = y; }
            }
        }
    }

    void UpdateTriangles()
    {
        triangles = new int[ xSize * zSize * 6 ];

        for (int z = 0,verts = 0, tris = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = verts + 0;
                triangles[tris + 1] = verts + xSize + 1;
                triangles[tris + 2] = verts + 1;
                triangles[tris + 3] = verts + 1;
                triangles[tris + 4] = verts + xSize + 1;
                triangles[tris + 5] = verts + xSize + 2;

                tris+= 6;
                verts++;
            }
            verts++;
        }
    }

    void UpdateColors()
    {
        colors = new Color[ vertices.Length ];
         for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float hieght = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
                colors[i++] = gradient.Evaluate(hieght);
            }
        }
    }

    [Serializable]
    public class Octave
    {
        public float amplitude;
        public float frequency;

        public Octave(float amplitude, float frequency)
        {
            this.amplitude = amplitude;
            this.frequency = frequency;
        }

    }

}