using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class SetTargetSheep : MonoBehaviour
{
    [SerializeField] private Transform target;

    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent= GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(!navMeshAgent.isActiveAndEnabled) { return; }
        navMeshAgent.destination = target.position;
        
      

    }

    public void setTarget(Transform target) { this.target = target; }
    public Transform getTarget() { return target; }
}
