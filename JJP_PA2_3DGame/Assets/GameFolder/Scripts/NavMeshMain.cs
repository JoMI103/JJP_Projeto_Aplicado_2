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
        updateMesh();
    }



    [SerializeField] private NavMeshSurface surface;

    bool updateMeshBool = false;

    [ContextMenu("updateMesh")]
    public void updateMesh()
    {
        updateMeshBool = true;
        //surface.BuildNavMesh();
       
    }
    
  
    private void LateUpdate() {
         if(updateMeshBool){ surface.UpdateNavMesh(surface.navMeshData); updateMeshBool = false;}
    }
    
    public void Build(){
        surface.BuildNavMesh();
    }
}