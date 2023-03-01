using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    [Range(2, 128)]
    public int resolution = 10;

    [SerializeField]
    MeshFilter meshFilter;
    MeshGenerator meshGenerator;


    private void OnValidate()
    {
        initialize();
        GenerateMesh();
    }


    void initialize()
    {
        if(meshFilter == null) meshFilter = new MeshFilter();
        
        if (meshFilter == null)
        {
            GameObject meshObj = new GameObject("mesh");
            meshObj.transform.parent = transform;

            meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
            meshFilter = meshObj.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = new Mesh();

        }
        meshGenerator = new MeshGenerator(meshFilter.sharedMesh, resolution, Vector3.up);
    }


    void GenerateMesh()
    {
        meshGenerator.constructMesh();
    }

}
