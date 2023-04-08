using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LocalGrid : MonoBehaviour
{
    [SerializeField]
    public enum normal {Ground,Wall,Roof }


    [SerializeField] public normal directionBuild;
    [SerializeField] private int GWidth = 10, GHeight = 10;
    [SerializeField] public Transform gridPanel;
    public GridXZ<GridObject> grid;
   
    private void Awake()
    {
        int gridWidth = GWidth;
        int gridHeight = GHeight;
        float cellSize = 2f;


        grid = new GridXZ<GridObject>(
            gridWidth,
            gridHeight,
            cellSize,
            transform.position,
            this.transform.localRotation,
            (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));

  
    }


    private void setUpGridPanel()
    {
        gridPanel.localScale = new Vector3(GWidth / 5f, 1, GHeight / 5f);
        gridPanel.localPosition = new Vector3(GWidth , 0, GHeight);
    }

    //DebugPerposes

    [SerializeField] bool debug;

    [SerializeField, Range(-90, 0)]
    float xRot, yRot;
    [SerializeField]bool Invert;

    [ContextMenu("SetUp")]
    private void setUp()
    {
        setUpGridPanel();
       // this.transform.localRotation = Quaternion.Euler(new Vector3(xRot, yRot, 0));
        if(Invert) { this.transform.localScale = new Vector3(1, -1, 1);
            Debug.Log(-gridPanel.up);
        } else { this.transform.localScale = new Vector3(1, 1, 1); Debug.Log(gridPanel.up); }
    }
}


public class GridObject
{
    private GridXZ<GridObject> grid;
    private int x, z;
    private PlacedBuilding placedBuilding;

    public GridObject(GridXZ<GridObject> grid, int x, int z)
    {
        this.grid = grid;
        this.x = x;
        this.z = z;
        placedBuilding = null;
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
        return placedBuilding == null;
    }

    public override string ToString()
    {
        return x + ", " + z + "\n" + placedBuilding;
    }
}