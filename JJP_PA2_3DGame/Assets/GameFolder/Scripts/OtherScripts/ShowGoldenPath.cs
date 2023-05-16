using UnityEngine;
using UnityEngine.AI;

public class ShowGoldenPath : MonoBehaviour
{
    public Transform target;
    private NavMeshPath path;
    private float elapsed = 0.0f;


    private void OnDrawGizmos() {
        
        path = new NavMeshPath();
            NavMeshQueryFilter navMeshQueryFilter = new NavMeshQueryFilter();
            navMeshQueryFilter.areaMask = NavMesh.AllAreas;
            NavMesh.SamplePosition(target.position,out NavMeshHit hit, 5,-1);
            NavMesh.CalculatePath(transform.position, hit.position, navMeshQueryFilter ,path);

        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
    }

}