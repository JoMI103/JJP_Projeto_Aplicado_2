using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityArrow : MonoBehaviour
{
    [SerializeField] Transform player;
    MeshRenderer mr;

    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        if (distance < 50f)
        {
            mr.material.SetInt("_isPerto", 1);
            

        }
        else
        {
            
            mr.material.SetInt("_isPerto", 0);
        }

    }
}
