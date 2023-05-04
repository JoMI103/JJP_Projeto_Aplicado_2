using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.LowLevel;
using static BuildingTypeSO;

public class PlayerBuild : MonoBehaviour
{
    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;
   // public event EventHandler OnObjectRemoved;
    public event EventHandler OnObjectPlacedRemoved;


    private PlayerLook playerLook;
    private InputManager inputManager;
    private PlayerStats playerStats;
    
    public BuildingTypeSO buildingTypeSO;
    private BuildingTypeSO.Dir dir = BuildingTypeSO.Dir.Down;
    
    [SerializeField] private float constructionDistance;
    [SerializeField] private LayerMask canConstruct, GridLayer;
    public Vector3 mouseWorldPos, mouseCurrentGridPos;
    public bool hitting;
 
    public LocalGrid currentGrid;

    private void Awake()
    {
        playerLook = GetComponent<PlayerLook>();
        playerStats = GetComponent<PlayerStats>();  
        buildingTypeSO = null;

    }

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        inputManager.onFoot.Place.performed += ctx => PlaceBuilding();
        inputManager.onFoot.Destroy.performed += ctx => DestroyBuilding();
        inputManager.onFoot.Rotate.performed += ctx => RotateBuilding();

        currentGrid = null;

        OnObjectPlacedRemoved += bakeNavMesh;
    }

    private void Update()
    {
        hitting = checkGrid();
        playerLook.GetMouseWorldPosition(constructionDistance,out mouseWorldPos,canConstruct);
        if (!hitting) return;
        
        mouseCurrentGridPos = currentGrid.transform.InverseTransformPoint(mouseWorldPos);  
    }

    #region navmesh

    private void bakeNavMesh(object sender, System.EventArgs e)
    {
        //NavMeshMain.Instance.build();
        Invoke("buildNavMesh", 0.1f);
    }

    private void buildNavMesh()
    {
        NavMeshMain.Instance.updateMesh();
    }

    #endregion

    private bool checkGrid()
    {
        
        Ray ray = playerLook.playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, constructionDistance, GridLayer))
        {
            LocalGrid lg = raycastHit.collider.transform.parent.parent.GetComponent<LocalGrid>();
            if (lg != null && lg != currentGrid) { currentGrid = lg; RefreshSelectedObjectType();  }
            return true;
        }
        else
        {
            currentGrid = null;
            return false;
        }
          
    }

    private void PlaceBuilding() {

        if (buildingTypeSO == null || currentGrid == null) return;

        if (!playerStats.checkResourcesQuantity(buildingTypeSO)) { Debug.LogWarning("No Resources "); return; }

        switch (currentGrid.directionBuild)
        {
            case LocalGrid.normal.Ground: if (!buildingTypeSO.Ground) return; break;
            case LocalGrid.normal.Wall: if (!buildingTypeSO.Wall) return; break;
            case LocalGrid.normal.Roof: if (!buildingTypeSO.Roof) return; break;  

        }

        
        currentGrid.grid.GetXZ(mouseCurrentGridPos, out int x, out int z);
        Vector2Int placedObjectOrigin = new Vector2Int(x, z);
        placedObjectOrigin = currentGrid.grid.ValidateGridPosition(placedObjectOrigin);


        List<Vector2Int> gridPositionList = buildingTypeSO.GetGridPosition(new Vector2Int(x, z), dir);
        bool canBuild = true;

        foreach (Vector2Int position in gridPositionList)
        {
            GridObject gridObject = currentGrid.grid.GetGridObject(position.x, position.y);
            if (gridObject == null || !gridObject.canBuild())
            { 
                canBuild = false;  
            }
        }
  
        if (!canBuild) { Debug.LogWarning("Can't build in " + x + ", " + z); return; }


       
        playerStats.useResources(buildingTypeSO);
        


        Vector2Int rotationOffset = buildingTypeSO.GetRotationOffSet(dir);
        

        Vector3 buildingWorldPosition = currentGrid.grid.GetLocalPosition(placedObjectOrigin.x + rotationOffset.x, 
            placedObjectOrigin.y + rotationOffset.y);

        PlacedBuilding placedBuilding =  PlacedBuilding.Create(currentGrid.transform,buildingWorldPosition, placedObjectOrigin, dir, buildingTypeSO);

        foreach (Vector2Int gridPosition in gridPositionList)
        {
            currentGrid.grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedBuilding(placedBuilding);
        }

        OnObjectPlaced?.Invoke(this, EventArgs.Empty);
        OnObjectPlacedRemoved?.Invoke(this, EventArgs.Empty);
        //DeselectObjectType();
    }

    private void DestroyBuilding()
    {
        if(!hitting) return;



            //Gets the object of the grid (TGridObject, in this case an GridObject(class created in this file))
            //with the mouse position
            GridObject gridObject = currentGrid.grid.GetGridObject(mouseCurrentGridPos);
        if (gridObject != null)
        {


            PlacedBuilding placedBuilding = gridObject.GetPlacedBuilding();

            if (placedBuilding != null)
            {
                placedBuilding.DestroySelf();
                List<Vector2Int> gridPositionList = placedBuilding.GetGridPositionList();

                //Clear the placebuilding data from the gridObjects
                foreach (Vector2Int position in gridPositionList)
                {
                    currentGrid.grid.GetGridObject(position.x, position.y).ClearPlacedBuilding();
                }
                OnObjectPlacedRemoved?.Invoke(this, EventArgs.Empty);

            }
        }

       
    }

    public void GetMouseWorldSnappedPosition(out Vector3 pos)
    {
        if (buildingTypeSO == null) { pos = mouseCurrentGridPos; return; } //segue o rato

        currentGrid.grid.GetXZ(mouseCurrentGridPos, out int x, out int z);
        Vector2Int rotationOffset = buildingTypeSO.GetRotationOffSet(dir);
        Vector3 buildObjectWorldPosition = currentGrid.grid.GetLocalPosition(x + rotationOffset.x, z + rotationOffset.y);
        pos = buildObjectWorldPosition;
   }
   
    private void RotateBuilding()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            dir = BuildingTypeSO.GetNextDir(dir);
        }
    }

    #region SOManagement

    public void setBuildingTypeSO(BuildingTypeSO so) { buildingTypeSO = so; RefreshSelectedObjectType(); }

    public void DeselectObjectType() { buildingTypeSO = null; RefreshSelectedObjectType(); }

    private void RefreshSelectedObjectType() { OnSelectedChanged?.Invoke(this, EventArgs.Empty); }   //REfresh the ghost object

    public BuildingTypeSO GetPlacedObjectTypeSO() { return buildingTypeSO; }

    #endregion

    public Quaternion GetPlacedObjectRotation()
    {
        if (buildingTypeSO != null)
            return Quaternion.Euler(0, buildingTypeSO.GetRotationAngle(dir), 0);
        else
            return Quaternion.identity;
    }

    public Vector3 GetPlacedObjectScale()
    {
        if (buildingTypeSO != null)
            return currentGrid.transform.localScale;
        else
            return new Vector3(1, 1, 1);
    }
}
