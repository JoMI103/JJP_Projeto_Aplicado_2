using UnityEngine;
using UnityEngine.AI;

public class ShowGoldenPath : MonoBehaviour
{
    public Transform target;
    private NavMeshPath path;
    private float elapsed = 0.0f;



    private void OnDrawGizmos() {
        
        path = new NavMeshPath();
        


            int n = NavMesh.AllAreas; n -= NavMesh.GetAreaFromName("Wall");

            NavMesh.CalculatePath(transform.position, target.position,n, path);

        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
    }
}