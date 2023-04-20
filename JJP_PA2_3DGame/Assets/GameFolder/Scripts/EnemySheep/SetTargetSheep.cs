using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class SetTargetSheep : MonoBehaviour
{
    [SerializeField] private Transform currentTarget;

    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent= GetComponent<NavMeshAgent>();
    }


    public void setTarget(Transform target) { currentTarget = target; navMeshAgent.SetDestination(target.position); }
    public Transform getTarget() { return currentTarget; }

  
}
