using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyObjectButtom : Interactable
{
    public GameObject gm;

    private Material mt;

    private void Start()
    {
        mt = gm.GetComponent<Renderer>().material;
    }


    [SerializeField] private int idMod;
    [SerializeField] private Color colorSet;

    protected override void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);

        Color c = mt.GetColor("_Color");
        float metal = mt.GetFloat("_Metallic"); ;
        switch (idMod)
        {
            case 0:  mt.SetColor("_Color", new Color(c.r, c.g, c.b, c.a += 0.1f)); break; 
            case 1:  mt.SetColor("_Color", new Color(c.r, c.g, c.b, c.a -= 0.1f)); break; 
            case 2: metal -= 0.1f; mt.SetFloat("_Metallic", metal); break; 
            case 3: metal += 0.1f; mt.SetFloat("_Metallic", metal); break; 
            case 4: mt.SetColor("_Color", new Color(colorSet.r, colorSet.g, colorSet.b, c.a -= 0.1f)); break;
            default: break;
        }   
    }
}
