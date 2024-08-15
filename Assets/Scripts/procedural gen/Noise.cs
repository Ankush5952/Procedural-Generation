using UnityEngine;

public static class Noise
{
    public static float[,] noiseMap;

    public static float[,] GenerateNoiseMap(int width, int length, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000,100000) + offset.x;
            float offsetY = prng.Next(-100000,100000) + offset.y;

            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        noiseMap = new float[width,length];

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for(int i = 0; i < octaves; i++)
                {
                    float X = (x - width/2f)/scale * frequency + octaveOffsets[i].x * frequency;
                    float Z = (z - length/2f)/scale * frequency + octaveOffsets[i].y * frequency;

                    float noiseValue = Mathf.PerlinNoise(X,Z) * 2 - 1; //noisevalue = [-1 , 1]
                    noiseHeight += noiseValue * amplitude;

                    if(noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight;
                    if(noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight;

                    amplitude *= persistance; // persistance = [0 , 1]
                    frequency *= lacunarity; // lacunarity = [1 , inf]
                }

                noiseMap[x,z] = noiseHeight;
                
            }
        }
        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                noiseMap[x,z] = Mathf.InverseLerp(minNoiseHeight,maxNoiseHeight,noiseMap[x,z]);
            }
        }

        return noiseMap;
    }
}
