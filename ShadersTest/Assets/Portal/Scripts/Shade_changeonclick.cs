using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shade_changeonclick : MonoBehaviour
{
    public Texture tex;
    private void OnMouseDown()
    {
        //this.GetComponent<Renderer>().material = matReplace;
        //this.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        this.GetComponent<Renderer>().material.SetTexture("_MainTex", tex);

    }
}
