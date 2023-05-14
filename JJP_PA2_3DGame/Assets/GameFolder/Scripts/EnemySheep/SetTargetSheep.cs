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
    
    private void Start() {
        StartCoroutine(calcPath());
    }

    private IEnumerator calcPath(){
        WaitForSeconds w  = new WaitForSeconds(1f);
        
        while (true)
        {
            if(sheepMeshAgent.enabled){

                path = new NavMeshPath();
                sheepMeshAgent.CalculatePath(tPos,path);
                sheepMeshAgent.SetPath(path);
            }
            yield return w;
        }
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
}
