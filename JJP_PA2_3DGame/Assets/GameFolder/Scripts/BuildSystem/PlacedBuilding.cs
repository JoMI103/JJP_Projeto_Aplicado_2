using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedBuilding : MonoBehaviour {

    public static PlacedBuilding Create(Transform grid,Vector3 worldPosition, Vector2Int origin, BuildingTypeSO.Dir dir, BuildingTypeSO buildingTypeSO) {
        Transform placedBuildingTransform = 
            Instantiate(buildingTypeSO.prefab,
            worldPosition,
            Quaternion.identity,
            grid);
        placedBuildingTransform.localRotation = Quaternion.Euler(0, buildingTypeSO.GetRotationAngle(dir), 0);
        placedBuildingTransform.localPosition= worldPosition;
        PlacedBuilding placedBuilding = placedBuildingTransform.GetComponent<PlacedBuilding>();

        placedBuilding.buildingTypeSO = buildingTypeSO;
        placedBuilding.origin= origin;
        placedBuilding.dir = dir;

        return placedBuilding;
    }



    private BuildingTypeSO buildingTypeSO;
    private Vector2Int origin;
    private BuildingTypeSO.Dir dir;


    public List<Vector2Int> GetGridPositionList()
    {
        return buildingTypeSO.GetGridPosition(origin,dir);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

}
