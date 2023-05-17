using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityDanger : MonoBehaviour
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
        distance *= 5;
        distance = Mathf.Clamp(distance, 0, 100);
        distance = distance / 100;
        distance = 1 - distance;
        Debug.Log(distance);
        mr.material.SetFloat("_playerDistance", distance);
    }

}
