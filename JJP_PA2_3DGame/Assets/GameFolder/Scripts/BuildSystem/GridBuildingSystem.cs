using System;
using System.Collections.Generic;
using UnityEngine;


public class GridBuildingSystem : MonoBehaviour
{


    [Header("GridDimensions")]
    [SerializeField] private int GWidth = 10, GHeight = 10;


    public static GridBuildingSystem Instance { get; private set; }

    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;

    //[SerializeField] private List<BuildingTypeSO> buildingTypeSOList = null;
    public BuildingTypeSO buildingTypeSO;

    private PlayerLook playerLook;
    private GridXZ<GridObject> grid;
    private BuildingTypeSO.Dir dir = BuildingTypeSO.Dir.Down;


    [SerializeField] private float constructionDistance;
     
    private void Awake()
    {
        Instance = this;
        playerLook = GetComponent<PlayerLook>();

        int gridWidth = GWidth;
        int gridHeight = GHeight;
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


    

    private void Update()
    {
        //Places the building 
        if(Input.GetMouseButtonDown(0) && buildingTypeSO != null){ // checks if has something to place

            //gets the coordinates in the grid of the mouse position
            grid.GetXZ(playerLook.GetMouseWorldPosition(constructionDistance), out int x, out int z);
            Vector2Int placedObjectOrigin = new Vector2Int(x, z);

            //checks if origin can be placed in the grid
            placedObjectOrigin = grid.ValidateGridPosition(placedObjectOrigin);


            //Gets the grid position list o the current building (like 1 by 2 = list[2])
            //and checks if the postions are free
            List<Vector2Int> gridPositionList = buildingTypeSO.GetGridPosition(new Vector2Int(x, z), dir);
            bool canBuild = true;
            foreach (Vector2Int position in gridPositionList) {
                if (!grid.GetGridObject(position.x, position.y).canBuild()){ //erro
                    canBuild = false;
                    break;
                }
            }

            if (canBuild) {

                //Gets the rotation and worldPosition of the building
                Vector2Int rotationOffset = buildingTypeSO.GetRotationOffSet(dir);
                Vector3 buildingWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) +
                    new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

                //Instatiantes a prefab with placedBuilding script
                PlacedBuilding placedBuilding = PlacedBuilding.Create(buildingWorldPosition, placedObjectOrigin, dir, buildingTypeSO);

                //Sets the placedBuilding on the other grid positions
                foreach(Vector2Int gridPosition in gridPositionList) {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedBuilding(placedBuilding);
                }

                //some events s
                OnObjectPlaced?.Invoke(this, EventArgs.Empty);

               
                //DeselectObjectType();
            }
            else {
                Debug.LogWarning("Can't build in " + x + ", " + z);
            }
        }

        //Deletes the building
        if (Input.GetMouseButtonDown(1))
        {
            //Gets mousePosition from PlayerLook function
            Vector3 mousePosition = playerLook.GetMouseWorldPosition(constructionDistance);


            //Gets the object of the grid (TGridObject, in this case an GridObject(class created in this file))
            //with the mouse position
            GridObject gridObject = grid.GetGridObject(mousePosition);
            if (gridObject != null) {

                
                PlacedBuilding placedBuilding = gridObject.GetPlacedBuilding();

                if (placedBuilding != null)
                {
                    //Destroy the buiding gameObject
                    placedBuilding.DestroySelf();

                    //gets the gridObjects positions of the building
                    List<Vector2Int> gridPositionList = placedBuilding.GetGridPositionList();

                    //Clear the placebuilding data from the gridObjects
                    foreach (Vector2Int position in gridPositionList) {
                        grid.GetGridObject(position.x, position.y).ClearPlacedBuilding();
                    }
                }
            }
        }

        //Rotates the building
        if (Input.GetKeyDown(KeyCode.R)){
            dir = BuildingTypeSO.GetNextDir(dir);
        }

        //Selects other buildings (Temp)
       // if (Input.GetKeyDown(KeyCode.Alpha1)) { buildingTypeSO = buildingTypeSOList[0]; RefreshSelectedObjectType(); }
       // if (Input.GetKeyDown(KeyCode.Alpha2)) { buildingTypeSO = buildingTypeSOList[1]; RefreshSelectedObjectType(); }
       // if (Input.GetKeyDown(KeyCode.Alpha0)) { DeselectObjectType(); }
    }


    public void setBuildingTypeSO(BuildingTypeSO so)
    {
        buildingTypeSO = so; RefreshSelectedObjectType();
    }

    public void DeselectObjectType()
    {
        buildingTypeSO = null; RefreshSelectedObjectType();
    }

    //REfresh the ghost object
    private void RefreshSelectedObjectType()
    {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        grid.GetXZ(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    public bool GetMouseWorldSnappedPosition(out Vector3 pos)
    {
        bool show = playerLook.GetMouseWorldPosition(constructionDistance,out Vector3 mousePosition);
        grid.GetXZ(mousePosition, out int x, out int z);
       

        if (buildingTypeSO != null)
        {
            Vector2Int rotationOffset = buildingTypeSO.GetRotationOffSet(dir);
            Vector3 buildObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            pos = buildObjectWorldPosition;
        }
        else pos = mousePosition;

        return show;
    }

    public Quaternion GetPlacedObjectRotation()
    {
        if (buildingTypeSO != null)
            return Quaternion.Euler(0, buildingTypeSO.GetRotationAngle(dir), 0);
        else
            return Quaternion.identity;
    }

    public BuildingTypeSO GetPlacedObjectTypeSO()
    {
        return buildingTypeSO;
    }
}