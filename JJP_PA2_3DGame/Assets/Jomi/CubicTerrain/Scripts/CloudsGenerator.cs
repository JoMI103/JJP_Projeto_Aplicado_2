using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsGenerator : MonoBehaviour
{
    [SerializeField] BoxCollider BoxCollider;
    [SerializeField] private int Clouds;

    [SerializeField] Transform cloudPrefab;

    [ContextMenu("Generate")]
    private void generateClouds()
    {
        int childs = transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < Clouds; i++)
        {
            Transform g = Instantiate(cloudPrefab,Vector3.zero,Quaternion.identity,this.transform);

            
            g.transform.position = transform.position + new Vector3(Random.Range(-1f, 1f) * (BoxCollider.size.x / 2),
                Random.Range(-1f, 1f) * (BoxCollider.size.y / 2),
                Random.Range(-1f, 1f) * (BoxCollider.size.z / 2));
        }
    }


   
    

}
