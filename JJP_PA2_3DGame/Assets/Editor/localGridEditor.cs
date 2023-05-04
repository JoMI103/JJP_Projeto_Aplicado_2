using log4net.Util;
using System;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;


[CustomEditor(typeof(LocalGrid))]
public class localGridEditor : Editor
{
    static Vector3 pos1, pos2;
    bool mode = false, edit = false;
    bool confirm = false;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        LocalGrid localGrid = (LocalGrid)target;
        if(!confirm)
        if (GUILayout.Button("SetUp"))
        {
            confirm = true;
        }

        if (confirm)
        {
            if (GUILayout.Button("No"))
            {
                confirm = false;
            }
            if (GUILayout.Button("Yes"))
            {
                confirm = false;
                localGrid.setUp();
            }
        }
        if (GUILayout.Button("BuildGridTiles"))
        {
            localGrid.buildTiles();
        }
    }

    public void OnSceneGUI()
    {
        LocalGrid localGrid = (LocalGrid)target;
        if(localGrid == null) return;

        if (EditorWindow.mouseOverWindow is SceneView && !(EditorWindow.focusedWindow is SceneView))
        {
            EditorWindow.FocusWindowIfItsOpen<SceneView>();
        }

        int width = localGrid.GWidth, height = localGrid.GHeight;


   


        switch (Event.current.type)
        {
            case EventType.KeyDown:
                switch (Event.current.keyCode)
                {
                    case KeyCode.C: edit = true; break;
                    case KeyCode.V: mode = true; break;
                    case KeyCode.B: mode = false; break;
                    default: break;
                }
                break;
            case EventType.KeyUp:
                switch (Event.current.keyCode)
                {
                    case KeyCode.C: edit = false; break;
                   
                    default: break;
                }
                break;

            case EventType.MouseDown: 
                if (!edit) return;
                    pos1 = pos2 = calcMousePos();  
                break;
            case EventType.MouseDrag:
                if (!edit) return;
                    pos2 = calcMousePos(); 
                break;        
            case EventType.MouseUp:
                if (edit)
                {
                    updateCanBuild(localGrid);
                }
                pos1 = pos2 = Vector3.zero;
                break;

            case EventType.Layout:
                if (!edit) return;
                Handles.RectangleHandleCap(10, localGrid.gridPanel.position,
                    localGrid.transform.rotation * Quaternion.Euler(90, 0, 0),
                    MathF.Max(width + 10, height + 10), EventType.Layout);
                break;

            case EventType.Repaint:
                if (!edit) return;
                Handles.color = Color.yellow;
                Handles.RectangleHandleCap(10, localGrid.gridPanel.position,
                   localGrid.transform.rotation * Quaternion.Euler(90, 0, 0),
                   MathF.Max(width+10, height+10), EventType.Repaint);
                if(edit && pos2 != Vector3.zero)drawCube(localGrid);
                break;
        }

       
    }


    void drawCube(LocalGrid localGrid)
    {

        Vector3 p1 = localGrid.transform.InverseTransformPoint(pos1);
        Vector3 p2 = localGrid.transform.InverseTransformPoint(pos2);
        Vector3 p3 = new Vector3(p1.x, 0, p2.z),
                p4 = new Vector3(p2.x, 0, p1.z);

        p1 = localGrid.transform.TransformPoint(p1);
        p2 = localGrid.transform.TransformPoint(p2);
        p3 = localGrid.transform.TransformPoint(p3);
        p4 = localGrid.transform.TransformPoint(p4);

        Color color = Color.red;
        if(mode) color = Color.green;

        using (new Handles.DrawingScope(color))
        {
            Handles.DrawLine(p1, p3, 5);
            Handles.DrawLine(p1, p4, 5);
            Handles.DrawLine(p2, p3, 5);
            Handles.DrawLine(p2, p4, 5);
            HandleUtility.Repaint();
        }
    }

    Vector3 calcMousePos()
    {

        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 1000.0f))
        {
            return  hit.point;
        }

        return Vector3.zero;
    }

    private void updateCanBuild(LocalGrid localGrid)
    {
  
 
        
        Undo.RecordObject(localGrid, "canbuild");
        Debug.Log("Calculando");
        if (pos1 == pos2)
        {
            Vector3 p = localGrid.transform.InverseTransformPoint(pos1) / 2;
            int x = Mathf.FloorToInt(p.x); int z = Mathf.FloorToInt(p.z);
            localGrid.canBuild[x, z] = mode;
            
        }
        else
        {
            Vector3 p1 = localGrid.transform.InverseTransformPoint(pos1) / 2, p2 = localGrid.transform.InverseTransformPoint(pos2) / 2;
            int x1 = Mathf.FloorToInt(p1.x); int z1 = Mathf.FloorToInt(p1.z);
            int x2 = Mathf.FloorToInt(p2.x); int z2 = Mathf.FloorToInt(p2.z);

            if (x2 < x1) { int aux = x1; x1 = x2; x2 = aux; }
            if (z2 < z1) { int aux = z1; z1 = z2; z2 = aux; }


            for(int x = x1; x <= x2; x++)
            {
                for (int z = z1; z <= z2; z++)
                {
                    localGrid.canBuild[x, z] = mode;
                }
            }
        }

        
    }


}
