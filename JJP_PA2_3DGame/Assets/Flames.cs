using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flames : MonoBehaviour
{
    private void Start()
    {
    
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if(other.gameObject.tag == "Tree")
        {            
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            Destroy(other.gameObject, 6);
            
            
        }

    }
 
}
