using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : MonoBehaviour
{
    [SerializeField,Range(3, 64)] private int resolution;
    [SerializeField,Range(2,1000)] private int layers;
    
    [SerializeField] private Material material;
    [SerializeField] MeshFilter meshFilter;

    public CylinderMesh cylinderMesh;
    

    
    
    void initialize()
    {
        if(meshFilter == null ) meshFilter = new MeshFilter();
    
        if (meshFilter == null)
        {
            GameObject meshObj = new GameObject("mesh");
            meshObj.transform.parent = transform;

            meshObj.AddComponent<MeshRenderer>().sharedMaterial = material;
            meshFilter = meshObj.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = new Mesh();

        }
        if(cylinderMesh == null) cylinderMesh = gameObject.AddComponent<CylinderMesh>();
            cylinderMesh.create(meshFilter.sharedMesh,layers,resolution);
        
    }
    
    void GenerateMesh()
    {  
        cylinderMesh.constructMesh();
    }





    [ContextMenu("GenerateCylinder")]
    public void GenerateCloud()
    {
        initialize();
        GenerateMesh();
    }
    
}
