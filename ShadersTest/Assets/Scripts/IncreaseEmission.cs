using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseEmission : MonoBehaviour
{
    public GameObject player;
    public float distance;

    private void Start()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
    }
    private void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        if(distance > 100)
        {
            this.GetComponent<Renderer>().material.SetFloat("_Slider", -10);
        }
        else
        {
            this.GetComponent<Renderer>().material.SetFloat("_Slider", 20);
        }
        
    }
    
}

/*
this.gameObject.GetComponent<Renderer>().material.SetFloat("_distance", Vector3.Distance(this.gameObject.transform.position, player.transform.position));
    }

raycastHit.collider.gameObject.GetComponent<Renderer>().material.SetVector("_centro", raycastHit.point);
*/