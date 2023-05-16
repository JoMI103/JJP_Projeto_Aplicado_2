using UnityEngine;
using UnityEngine.AI;

public class ShowGoldenPath : MonoBehaviour
{
    public Transform target;
    private NavMeshPath path;
    private float elapsed = 0.0f;

    [SerializeField] catmullRomSpline catmullRomSpline;

    [ContextMenu("CalcPath")]
    public void calcPath(){
         path = new NavMeshPath();
            NavMeshQueryFilter navMeshQueryFilter = new NavMeshQueryFilter();
            navMeshQueryFilter.areaMask = NavMesh.AllAreas;
            NavMesh.SamplePosition(target.position,out NavMeshHit hit, 5,-1);
            NavMesh.CalculatePath(transform.position, hit.position, navMeshQueryFilter ,path);
            
            catmullRomSpline.setCorners(path.corners);
    }


   

}