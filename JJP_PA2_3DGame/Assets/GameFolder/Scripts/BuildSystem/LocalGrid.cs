using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class LocalGrid : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField] public enum normal {Ground,Wall,Roof }
    [SerializeField] public normal directionBuild;

     public int GWidth = 10, GHeight = 10;
     public GridXZ<GridObject> grid;
     public bool[,] canBuild;
    public bool[] canBuild2;
    public Vector3[,] nodesPositions;

    [SerializeField] public Transform gridPanel;
    [SerializeField] Transform parentTiles;


    private void Awake()
    {
        parentTiles.gameObject.SetActive(true);

        int gridWidth = GWidth;
        int gridHeight = GHeight;
        float cellSize = 2f;


        grid = new GridXZ<GridObject>(
            gridWidth,
            gridHeight,
            cellSize,
            transform.position,
            this.transform.localRotation,
            canBuild,
            (GridXZ<GridObject> g, int x, int z, bool c) => new GridObject(g, x, z, c));
            
            Destroy(gridPanel.gameObject);
    }


    private void setUpGridPanel()
    {
        gridPanel.localScale = new Vector3(GWidth / 5f, 1, GHeight / 5f);
        gridPanel.localPosition = new Vector3(GWidth , 0, GHeight);
    }

    [SerializeField]bool Invert , startCanBuild;


#if UNITY_EDITOR
    [ContextMenu("SetUp")]
    public void setUp()
    {
        setUpGridPanel();
        if(Invert) { this.transform.localScale = new Vector3(1, -1, 1);
            Debug.Log(-gridPanel.up);
        } else { this.transform.localScale = new Vector3(1, 1, 1); Debug.Log(gridPanel.up); }

        resetCanBuildData();
        calcPositionData();


    }
    //DebugPerposes


    [ContextMenu("ReserCanBuildData")]
    private void resetCanBuildData()
    {
        canBuild = new bool[GWidth, GHeight];
        for (int x = 0; x < GWidth; x++)
            for (int z = 0; z < GHeight; z++)
            {
                canBuild[x, z] = startCanBuild;
            }
    }

    [ContextMenu("CalcPostionData")]
    private void calcPositionData()
    {
        nodesPositions = new Vector3[GWidth, GHeight];

        for (int x = 1; x <= GWidth; x++)
            for (int z = 1; z <= GHeight; z++)
            {
                nodesPositions[x - 1, z - 1] = new Vector3(x * 2 - 1, 0, z * 2 - 1); // transform.localToWorldMatrix.rotation * new Vector3(x * 2 - 1, 0, z * 2 - 1) + transform.position;
            }
    }


    [SerializeField] Transform tile;

    [ContextMenu("BuildTiles")]

    public void buildTiles()
    {
        int childs = parentTiles.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(parentTiles.GetChild(i).gameObject);
        }
        
        for (int x = 0; x < GWidth; x++)
            for (int z = 0; z < GHeight; z++)
            {
                if (canBuild[x,z]) Instantiate(tile, parentTiles.TransformPoint( nodesPositions[x, z]), transform.rotation, parentTiles);
            }
        
    }

    [SerializeField] bool debug;
[SerializeField]  float alpha = 0.7f, scale = 0.95f;

    private void OnDrawGizmos()
    {
        if (!Selection.Contains(gameObject) && debug) //Debug
        {
            Gizmos.color = new Color(0, 1 -  (transform.position.y % 3 / 3), 0, alpha);
            Gizmos.matrix = transform.localToWorldMatrix;
            for (int x = 0; x < GWidth; x++)
                for (int z = 0; z < GHeight; z++)
                {
                    if (canBuild[x, z]) Gizmos.DrawCube(nodesPositions[x, z], new Vector3(1.9f, 0.5f, 1.9f));
                }
        }
    }

    private void OnDrawGizmosSelected()
    {
        
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.matrix = transform.localToWorldMatrix;
            for (int x = 0; x < GWidth; x++)
                for (int z = 0; z < GHeight; z++)
                {

                    if (canBuild[x, z]) Gizmos.DrawCube(nodesPositions[x, z], new Vector3(1.9f, 0.5f, 1.9f));
                }
       
    }

#endif

    //public Vector3[,] unserializable = new Vector3[3, 3];

    // A list that can be serialized
    [SerializeField, HideInInspector] private List<Package<bool>> serializableBoolCanBuild;
    [SerializeField, HideInInspector] private List<Package<Vector3>> serializableVector3Pos;
    // A package to store our stuff
    [System.Serializable]
    struct Package<TElement>
    {
        public int Index0;
        public int Index1;
        public TElement Element;
        public Package(int idx0, int idx1, TElement element)
        {
            Index0 = idx0;
            Index1 = idx1;
            Element = element;
        }
    }

    public void OnBeforeSerialize()
    {
        // Convert our unserializable array into a serializable list
        serializableBoolCanBuild = new List<Package<bool>>();
        serializableVector3Pos = new List<Package<Vector3>>();
        for (int i = 0; i < canBuild.GetLength(0); i++)
        {
            for (int j = 0; j < canBuild.GetLength(1); j++)
            {
                serializableBoolCanBuild.Add(new Package<bool>(i, j, canBuild[i, j]));
                serializableVector3Pos.Add(new Package<Vector3>(i, j, nodesPositions[i, j]));
            }
        }
    }
    
    public void OnAfterDeserialize()
    {
        // Convert the serializable list into our unserializable array
        canBuild = new bool[GWidth, GHeight];

        foreach (var package in serializableBoolCanBuild)
        {
            canBuild[package.Index0, package.Index1] = package.Element;
        }

        nodesPositions = new Vector3[GWidth, GHeight];  

        foreach (var package in serializableVector3Pos)
        {
            nodesPositions[package.Index0, package.Index1] = package.Element;
        }
    }

}


public class GridObject
{
    private GridXZ<GridObject> grid;
    private int x, z;
    private PlacedBuilding placedBuilding;
    private bool canBuildB;

    public GridObject(GridXZ<GridObject> grid, int x, int z, bool canBuildB)
    {
        this.grid = grid;
        this.x = x;
        this.z = z;
        placedBuilding = null;
        this.canBuildB = canBuildB;
    }

    public void SetPlacedBuilding(PlacedBuilding placedBuilding)
    {
        this.placedBuilding = placedBuilding;
        grid.TriggerGridObjectChanged(x, z);
    }

    public PlacedBuilding GetPlacedBuilding() { return placedBuilding; }

    public void ClearPlacedBuilding()
    {
        this.placedBuilding = null;
        grid.TriggerGridObjectChanged(x, z);
    }

    public bool canBuild()
    {
        if(!canBuildB) return false;
        return placedBuilding == null;
    }

    public override string ToString()
    {
        return x + ", " + z + "\n" + placedBuilding;
    }
}