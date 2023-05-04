using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud: MonoBehaviour
{
    [Range(2, 5)]
    public int resolution;
    public Material material;

    [SerializeField] MeshFilter[] meshFilters;
    CloudFace[] cloudFaces;


    public void newCloud(Material m ,int res, Vector3 size, Quaternion rotation)
    {
        material = m;
        resolution = res;
        GenerateCloud();
        transform.localScale = size;
        transform.rotation = rotation;
    }

    void initialize()
    {
        if(meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }

        cloudFaces = new CloudFace[6];
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.right, Vector3.left, Vector3.forward, Vector3.back };

        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = material;
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();

            }
                cloudFaces[i] = new CloudFace(meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }
    
    void GenerateMesh()
    {
        foreach(CloudFace cf in cloudFaces)
        {
            cf.constructMesh();
        }
    }



    [ContextMenu("GenerateCloud")]
    public void GenerateCloud()
    {
        initialize();
        GenerateMesh();
    }


}
