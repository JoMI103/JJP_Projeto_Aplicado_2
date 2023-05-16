using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class SetTargetSheep : MonoBehaviour
{
    private NavMeshPath path; private Vector3 tPos;
    
    private NavMeshAgent sheepMeshAgent;

    private void Awake() {
        path = new NavMeshPath();
        sheepMeshAgent= GetComponent<NavMeshAgent>();
        
    }
    

    public void setStaticTarget(Vector3 target)
    {
        tPos = target;
        
        if(sheepMeshAgent.enabled){
            path = new NavMeshPath();
        
            sheepMeshAgent.CalculatePath(tPos,path);
            sheepMeshAgent.SetPath(path);
        }
    }

    private void OnDrawGizmos() {
        if(sheepMeshAgent == null) return;
        NavMeshPath p  = sheepMeshAgent.path;

        for (int i = 0; i < p.corners.Length - 1; i++)
            Debug.DrawLine(p.corners[i], p.corners[i + 1],Color.yellow);
    }
}


