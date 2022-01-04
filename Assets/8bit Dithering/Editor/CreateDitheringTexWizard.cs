// Author: O.V.Pavlov http://ovpavlov.blogspot.ru/ 
using UnityEditor;
using UnityEngine;

public class CreateDitheringTexWizard: ScriptableWizard
{
    public int levels = 1;
    public string path = "dithering_8_Steps.png";
    public bool random;
    int lastlevels;
    bool lastRandom;
    [MenuItem("Assets/Create/Dithering Texture")]
    public static void CreateWizard()
    {
        DisplayWizard<CreateDitheringTexWizard>("Create Dithering Texture", "Create");
    }
    void OnWizardCreate()
    {
        SaveTexture(random ? GetRandomTex(levels) : GetTex(levels), @"Assets\" + path);
    }
    void OnWizardUpdate()
    {
        if (lastlevels == levels && lastRandom == random) return;
        levels = Mathf.Clamp(levels, 0, 8);
        int side = 2 << levels;

        if (path.Substring(0, 10) == "dithering_")
            path = "dithering_" + side + "x" + side +"_"+ (side * side) + (random ? "_Steps(Random).png" : "_Steps.png");
        lastlevels = levels;
        lastRandom = random;
    } 
    static void SaveTexture(Texture2D texture, string path)
    {
        string directoryName = System.IO.Path.GetDirectoryName(path);
        if (!System.IO.Directory.Exists(directoryName))
            System.IO.Directory.CreateDirectory(directoryName);

        byte[] bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, bytes);
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

        TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(path);
        textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
        textureImporter.npotScale = TextureImporterNPOTScale.None;
        textureImporter.isReadable = true;
        textureImporter.mipmapEnabled = false;
        textureImporter.filterMode = FilterMode.Point;
        textureImporter.textureType = TextureImporterType.Default;
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
    }

    //core below
    static Texture2D GetTex(int pov2)
    {
        int side = 2 << pov2;
        float[,] values = new float[side,side];
        float step = 1f / (side * side + 1);

        SetTexQuad(values, 0, 0, pov2, step * 0.5f, step);

        Texture2D tex = new Texture2D(side, side);
        for (int x = 0; x < side; x++)
            for (int y = 0; y < side; y++)
                tex.SetPixel(x, y, new Color(values[x, y], values[x, y], values[x, y]));
        tex.Apply();
        return tex;
    }
    static void SetTexQuad(float[,] values, int x, int y, int depth, float startVal, float step)
    {
        if (depth == 0)
        {
            float temp = startVal;
            values[x, y] = temp;
            temp += step;
            values[x + 1, y + 1] = temp;
            temp += step;
            values[x, y + 1] = temp;
            temp += step;
            values[x + 1, y] = temp;
        }
        else
        {
            int texelStep = 1 << depth;
            float temp = startVal;
            float newStep = step * 4;
            depth--;

            SetTexQuad(values, x, y, depth, temp, newStep);
            temp += step;
            SetTexQuad(values, x + texelStep, y + texelStep, depth, temp, newStep);
            temp += step;
            SetTexQuad(values, x, y + texelStep, depth, temp, newStep);
            temp += step;
            SetTexQuad(values, x + texelStep, y, depth, temp, newStep);
        }
    }
    static Texture2D GetRandomTex(int pov2)
    {
        int side = 2 << pov2;
        int length = side * side;
        Color[] colors = new Color[length];
        bool[] assigned = new bool[length];

        float step = 1f / (length + 1);
        float temp = step * 0.5f;
        int qwe = 0;
        for (int i = 0; i < length; i++)
        {
            while (true)
            {
                int x = Random.Range(0, length);
                if (!assigned[x])
                {
                    colors[x] = new Color(temp, temp, temp, 1f);
                    assigned[x] = true;
                    break;
                }
                else qwe++;
            }
            temp += step;
        }
        Texture2D tex = new Texture2D(side, side);
        tex.SetPixels(colors);
        tex.Apply();
        return tex;
    }
}