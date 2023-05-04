using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainInfo : MonoBehaviour
{
    [SerializeField] TerrainData terrainData;
    [SerializeField] RawImage rawImage;


    [ContextMenu("test")]
    private void test()
    {
        RenderTexture ok = terrainData.heightmapTexture;

        /*for(int x = 0; x < ok.width; x++)
        {
            for(int y = 0; y < ok.height; y++)
            {
            }
        }*/

        Debug.Log(ok.width);
        rawImage.texture = ok;
    }

}
