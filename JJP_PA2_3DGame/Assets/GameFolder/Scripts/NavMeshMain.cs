using UnityEngine;
using UnityEngine.AI;

public class NavMeshMain : MonoBehaviour
{
    public static NavMeshMain Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        surface = GetComponent<NavMeshSurface>();
    }



    [SerializeField] private NavMeshSurface surface;

    [ContextMenu("updateMesh")]
    public void updateMesh()
    {
        //surface.BuildNavMesh();
        surface.UpdateNavMesh(surface.navMeshData);
    }
    
    public void Build(){
        surface.BuildNavMesh();
    }
}