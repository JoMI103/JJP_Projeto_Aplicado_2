using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pATINHO : MonoBehaviour
{
    private void OnMouseDown()
    {
        this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        //this.GetComponent<Renderer>().material.SetFloat("_transparency", 0.1f);
        //this.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
    }
}
