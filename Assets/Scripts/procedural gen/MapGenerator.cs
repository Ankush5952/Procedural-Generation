using UnityEngine;

public class MapGenerator:MonoBehaviour
{
    public enum DrawMode{Map, Mesh};
    public DrawMode drawMode;
    public int mapWidth = 20;
    public int mapLength = 20;
    public float noiseScale = 0.3f;
    public float heightMultiplier = 1f;
    [Range(0,50)]
    public int octaves = 1;
    [Range(0,1)]
    public float persistance = 0.3f;
    public float lacunarity = 1;
    public int seed = 0;
    public Vector2 offset;
    public Gradient gradient;
    public bool autoUpdate = false;
    public AnimationCurve heightCurve;

    void Start()
    {
        GenerateMap();
    }


    public void GenerateMap()
    {
        MeshGen.heightMultiplier = heightMultiplier;
        MeshGen.heightCurve = heightCurve;

       float [,] map = Noise.GenerateNoiseMap(mapWidth,mapLength, seed, noiseScale, octaves, persistance, lacunarity, offset);

       MapDisplay display = FindObjectOfType<MapDisplay>();

       if(drawMode == DrawMode.Map) display.DisplayMap(map, gradient);

       else if(drawMode == DrawMode.Mesh) display.DrawMesh(map, MeshGen.GenerateTerrainMesh(map));

    }

    void OnValidate()
    {
        if(noiseScale <= 0) noiseScale = 0.001f;
        if(mapWidth < 1) mapWidth = 1;
        if(mapLength < 1) mapLength = 1;
        if(lacunarity < 1) lacunarity = 1f;
        if(heightMultiplier <= 0) heightMultiplier = 0.1f;;
    }

    
}