using System;
using System.Collections.Generic;
using UnityEngine;


public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem Instance { get; private set; }

    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;

    [SerializeField] private List<BuildingTypeSO> buildingTypeSOList = null;
    public BuildingTypeSO buildingTypeSO;

    private PlayerLook playerLook;
    private GridXZ<GridObject> grid;
    private BuildingTypeSO.Dir dir = BuildingTypeSO.Dir.Down;

     
    private void Awake()
    {
        Instance = this;
        playerLook = GetComponent<PlayerLook>();

        int gridWidth = 10;
        int gridHeight = 10;
        float cellSize = 2f;

    
        grid = new GridXZ<GridObject>(
            gridWidth, 
            gridHeight, 
            cellSize, 
            Vector3.zero, 
            (GridXZ<GridObject> g,int x,int z) => new GridObject(g, x, z));

        buildingTypeSO = null;
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

        public void SetPlacedBuilding(PlacedBuilding placedBuilding) { 
            this.placedBuilding = placedBuilding; 
            grid.TriggerGridObjectChanged(x, z); 
        }

        public PlacedBuilding GetPlacedBuilding() { return placedBuilding; }

        public void ClearPlacedBuilding() { 
            this.placedBuilding = null; 
            grid.TriggerGridObjectChanged(x, z); 
        }

        public bool canBuild() { 
            return placedBuilding == null; 
        }

        public override string ToString()
        {
            return x + ", " + z + "\n" + placedBuilding;
        }
    }


    private Vector3 getMousePosition()
    {
        return playerLook.GetMouseWorldPosition_Instance(5f);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && buildingTypeSO != null){
            
            grid.GetXZ(getMousePosition(), out int x, out int z);

            Vector2Int placedObjectOrigin = new Vector2Int(x, z);
            placedObjectOrigin = grid.ValidateGridPosition(placedObjectOrigin);


            List<Vector2Int> gridPositionList = buildingTypeSO.GetGridPosition(new Vector2Int(x, z), dir);
            bool canBuild = true;
            foreach (Vector2Int position in gridPositionList) {
                if (!grid.GetGridObject(position.x, position.y).canBuild()){
                    canBuild = false;
                    break;
                }
            }

            if (canBuild) {

                Vector2Int rotationOffset = buildingTypeSO.GetRotationOffSet(dir);
                Vector3 buildingWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) +
                    new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

                PlacedBuilding placedBuilding = PlacedBuilding.Create(buildingWorldPosition, placedObjectOrigin, dir, buildingTypeSO);

                foreach(Vector2Int gridPosition in gridPositionList) {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedBuilding(placedBuilding);
                }

                OnObjectPlaced?.Invoke(this, EventArgs.Empty);
                DeselectObjectType();
            }
            else {
                Debug.LogWarning("Can't build in " + x + ", " + z);
            }



        }


        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePosition = getMousePosition();
            if (grid.GetGridObject(mousePosition) != null)
            {

                PlacedBuilding placedBuilding = grid.GetGridObject(mousePosition).GetPlacedBuilding();

                if (placedBuilding != null)
                {
                    placedBuilding.DestroySelf();

                    List<Vector2Int> gridPositionList = placedBuilding.GetGridPositionList();
                    foreach (Vector2Int position in gridPositionList)
                    {
                        grid.GetGridObject(position.x, position.y).ClearPlacedBuilding();
                    }
                }
            }
        }

      


        if (Input.GetKeyDown(KeyCode.R)){
            dir = BuildingTypeSO.GetNextDir(dir);
        }


        if (Input.GetKeyDown(KeyCode.Alpha1)) { buildingTypeSO = buildingTypeSOList[0]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { buildingTypeSO = buildingTypeSOList[1]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha0)) { DeselectObjectType(); }
    }

    private void DeselectObjectType()
    {
        buildingTypeSO = null; RefreshSelectedObjectType();
    }

    private void RefreshSelectedObjectType()
    {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        grid.GetXZ(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    public Vector3 GetMouseWorldSnappedPosition()
    {
        Vector3 mousePosition = getMousePosition();
        grid.GetXZ(mousePosition, out int x, out int z);

        if (buildingTypeSO != null)
        {
            Vector2Int rotationOffset = buildingTypeSO.GetRotationOffSet(dir);
            Vector3 buildObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            return buildObjectWorldPosition;
        }
        else
        {
            return mousePosition;
        }
    }

    public Quaternion GetPlacedObjectRotation()
    {
        if (buildingTypeSO != null)
        {
            return Quaternion.Euler(0, buildingTypeSO.GetRotationAngle(dir), 0);
        }
        else
        {
            return Quaternion.identity;
        }
    }

    public BuildingTypeSO GetPlacedObjectTypeSO()
    {
        return buildingTypeSO;
    }


}

