using UnityEngine;

public class GridTexture : MonoBehaviour
{
    public int gridSize = 20;
    public Color colorA = new Color(0.1f, 0.1f, 0.1f);
    public Color colorB = new Color(0.4f, 0.4f, 0.4f);

    void Start()
    {
        Texture2D tex = new Texture2D(2, 2);
        tex.SetPixel(0, 0, colorA);
        tex.SetPixel(1, 1, colorA);
        tex.SetPixel(0, 1, colorB);
        tex.SetPixel(1, 0, colorB);
        tex.filterMode = FilterMode.Point;
        tex.Apply();

        Renderer rend = GetComponent<Renderer>();
        rend.material.mainTexture = tex;
        rend.material.mainTextureScale = new Vector2(gridSize, gridSize);
    }
}