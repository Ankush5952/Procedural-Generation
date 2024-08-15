using UnityEngine;

public static class TextureGenerator
{
    public static FilterMode filterMode;
    public static TextureWrapMode wrapMode;

    public static Texture2D GenerateTexture(float[,] noiseMap, Gradient gradient)
    {
        int width = noiseMap.GetLength(0);
        int length = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width, length);

        Color32[] colors= new Color32[width * length];
        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Color32 color = gradient.Evaluate(noiseMap[x,z]); //Color32.Lerp(Color.black,Color.white, noiseMap[x,z]);

                colors[z*width + x] = color;
            }
        }

        
        texture.SetPixels32(colors);
        texture.Apply();
        texture.filterMode = filterMode;
        texture.wrapMode =  wrapMode;

        return texture;
    }
}