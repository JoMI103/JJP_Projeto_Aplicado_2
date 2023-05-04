using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class SceneUpdateNavMesh : MonoBehaviour
{
    [SerializeField] NavMeshMain navMeshMain;
    
    private void Start() {
        navMeshMain = GetComponent<NavMeshMain>();
    }
    
    public void build(){
        navMeshMain.Build();
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(SceneUpdateNavMesh))]
public class SceneUpdateNavMeshEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        SceneUpdateNavMesh sceneUpdateNav = (SceneUpdateNavMesh)target;
         if (GUILayout.Button("Build"))
        {
            sceneUpdateNav.build();
        }
    }
}
#endif