using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsGenerator : MonoBehaviour
{
    [SerializeField] BoxCollider BoxCollider;
    [SerializeField] private int Clouds;
    [SerializeField] private Material material;
    [SerializeField] private float scale;

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
            GameObject g = new GameObject();
             int n  = Random.Range(3, 6);

            g.transform.parent = transform;
           

            switch (n)
            {
                case 3: g.AddComponent<Cloud>().newCloud(material, 3,
                    new Vector3(Random.Range(4f, 5f), Random.Range(4f, 5f), Random.Range(4f, 5f)) * scale,
                   Quaternion.identity);// Quaternion.Euler(new Vector3(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180))));
                    Debug.Log("3"); break;
                case 4: g.AddComponent<Cloud>().newCloud(material,4,
                   new Vector3(Random.Range(5f, 6f), Random.Range(5f, 6f), Random.Range(5f, 6f)) * scale,
                    Quaternion.identity);//Quaternion.Euler(new Vector3(Random.Range(0,180), Random.Range(0, 180), Random.Range(0, 180)))); 
                    Debug.Log("4"); break;
                case 5: g.AddComponent<Cloud>().newCloud(material,5,
                    new Vector3(Random.Range(6f, 7f), Random.Range(6f, 7f), Random.Range(6f, 7f)) * scale,
                    Quaternion.identity);//Quaternion.Euler(new Vector3(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180))));
                    Debug.Log("5"); break;
            }
            g.transform.position = transform.position + new Vector3(Random.Range(-1f, 1f) * (BoxCollider.size.x / 2),
                Random.Range(-1f, 1f) * (BoxCollider.size.y / 2),
                Random.Range(-1f, 1f) * (BoxCollider.size.z / 2));
        }
    }


   
    

}
