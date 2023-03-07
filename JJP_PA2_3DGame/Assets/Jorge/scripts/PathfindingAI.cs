using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathfindingAI : MonoBehaviour
{
    public Transform target;
    public float speed = 3f;
    public float stoppingDistance = 1f;

    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (target != null)
        {
            navMeshAgent.SetDestination(target.position);

            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= stoppingDistance)
            {
                navMeshAgent.isStopped = true;
            }
            else
            {
                navMeshAgent.isStopped = false;
            }
        }
    }
}