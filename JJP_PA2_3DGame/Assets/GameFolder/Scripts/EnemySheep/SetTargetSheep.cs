using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class SetTargetSheep : MonoBehaviour
{
    WaitForSeconds wait = new WaitForSeconds(0.1f);
    private Transform currentTarget;
    private NavMeshAgent navMeshAgent;

    private void Awake() {
        navMeshAgent= GetComponent<NavMeshAgent>();
    }

    private IEnumerator FollowTarget() {
        while (enabled)
        {
            navMeshAgent.SetDestination(currentTarget.position);
            yield return wait;
        }
        
    }


    public void setStaticTarget(Transform target)
    {
        StopAllCoroutines(); currentTarget = target;

        var path = new NavMeshPath();
        if (navMeshAgent.enabled)
            navMeshAgent.CalculatePath(currentTarget.position, path);

        navMeshAgent.SetPath(path);

    }

    public void setMovingTarget(Transform target) { 
        StopAllCoroutines(); 
        currentTarget = target; 
        StartCoroutine(FollowTarget()); 
    }

    public Transform getTarget() { return currentTarget; }

  
}
