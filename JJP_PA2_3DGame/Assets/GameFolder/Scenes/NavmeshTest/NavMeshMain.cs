

using System.Collections;
using System.Collections.Generic;
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



    private NavMeshSurface surface;

    [ContextMenu("Build")]
    public void build()
    {
        Debug.LogError("BUILDINNG");

        //surface.BuildNavMesh();
        surface.UpdateNavMesh(surface.navMeshData);
      
        
    }
    

}