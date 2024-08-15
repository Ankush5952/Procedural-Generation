using UnityEngine;

public class MapDisplay: MonoBehaviour
{
    public Renderer textureRenderer;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public FilterMode filterMode;
    public TextureWrapMode wrapMode;

    bool playmode = false;

    void OnApplicationQuit()
    {
        playmode = false;
    }

    void Awake()
    {
        playmode = true;
    }

    public void DisplayMap(float[,] noiseMap, Gradient gradient)
    {
        int width = noiseMap.GetLength(0);
        int length = noiseMap.GetLength(1);

        TextureGenerator.filterMode = filterMode;
        TextureGenerator.wrapMode = wrapMode;

        Texture2D texture = TextureGenerator.GenerateTexture(noiseMap,gradient);

        textureRenderer.sharedMaterial.mainTexture = texture;
        
        if(playmode) textureRenderer.material.mainTexture = texture;

        else textureRenderer.transform.localScale = new Vector3(width,1,length);
    }

    public void DrawMesh(float[,] noiseMap, MeshData meshData)
    {

        if(playmode)
        {
            meshFilter.mesh = meshData.CreateMesh();
        }else
        {
            meshFilter.sharedMesh = meshData.CreateMesh();
        }
    }

    
}