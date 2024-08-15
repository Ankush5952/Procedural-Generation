using UnityEngine;

public static class MeshGen
{
    public static float heightMultiplier = 1f;
    public static AnimationCurve heightCurve;

    public static MeshData GenerateTerrainMesh(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int length = heightMap.GetLength(1);
        float topLeftX = (width-1)/-2f;
        float topLeftZ = (length - 1)/2f;

        MeshData meshData = new MeshData(width, length);
        int vert = 0;

        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                meshData.vertices[vert] = new Vector3( topLeftX + x, heightCurve.Evaluate(heightMap[x,z]) * heightMultiplier, topLeftZ - z);
                
                meshData.uvs[vert] = new Vector2((float)x/width, (float)z/length );

                if(x < width - 1 && z < length - 1)
                {
                    meshData.AddTriangle(vert, vert + width + 1, vert + width);
                    meshData.AddTriangle(vert + width + 1, vert, vert + 1);
                }
                vert++;
            }
        }
        return meshData;
    }
}

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int tris = 0;

    public MeshData(int meshWidth, int meshLength)
    {
        this.vertices = new Vector3[ (meshWidth) * (meshLength) ];
        this.triangles = new int[ (meshWidth - 1) * (meshLength - 1) * 6];
        this.uvs = new Vector2[meshWidth * meshLength];
    }

    public void AddTriangle(int vertex1, int vertex2, int vertex3)
    {
        triangles[tris] = vertex1;
        triangles[tris + 1] = vertex2;
        triangles[tris + 2] = vertex3;

        tris += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();

        return mesh;
    }
}
