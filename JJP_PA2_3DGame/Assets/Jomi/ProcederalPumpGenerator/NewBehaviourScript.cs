using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NewBehaviourScript : MonoBehaviour
{
    [Range(0,1)]
    public float threshold;

    public Transform target;

    private void OnDrawGizmosSelected() {
        /* 
        Gizmos.DrawSphere(target.position,0.1f);
        bool isIn = Vector3.Distance(target.position,transform.position) < radius;
        if(isIn) Gizmos.color = Color.green; else Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,radius);
        */
 
        
        
        /* 
        Vector3 directionToTarget= (target.position - transform.position  ).normalized;
        
        Gizmos.color = Color.blue; Gizmos.DrawLine(transform.position,transform.position + directionToTarget);
        float dotValue = Vector3.Dot(transform.forward, directionToTarget);
        bool isIn = dotValue >= threshold;

        if(isIn) Gizmos.color = Color.green; else Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,transform.position + transform.forward); 
        */
        
    }
}
