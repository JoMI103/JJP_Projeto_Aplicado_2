using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blit : MonoBehaviour
{
    public Material mat;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, mat);
    }
    
    public void setHP(float coiso){
        mat.SetFloat("_DamageActive",coiso );
    }
}
