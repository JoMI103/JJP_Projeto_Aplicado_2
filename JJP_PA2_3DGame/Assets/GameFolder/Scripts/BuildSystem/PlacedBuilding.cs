using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedBuilding : MonoBehaviour{

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

    private void Start() {
        SetStats();
    }
    private void SetStats()
    {
        health = buildingTypeSO.health;
        baseHealth = health;
    }

    public BuildingTypeSO buildingTypeSO;
    private Vector2Int origin;
    public BuildingTypeSO.Dir dir;

    private int baseHealth;
    private int health;

    //You can add some methot from another script to be called by this delegates with onDestroyEvent += methotd;
    public delegate void DestroyEvent();
    public DestroyEvent onDestroyEvent;
    public delegate void TakeDamageEvent(int damage, int health);
    public TakeDamageEvent onTakeDamageEvent;

    [SerializeField] private UIHealthBar healthBar;

    public void takeDamge(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
           // healthBar.SetHealthBarPercentage(0);
            DestroySelf();
        }
        else
        {
            healthBar.SetHealthBarPercentage((float)health / baseHealth);
            onTakeDamageEvent?.Invoke(damage, health);
        }
    }


    public List<Vector2Int> GetGridPositionList()
    {
        return buildingTypeSO.GetGridPosition(origin,dir);
    }

    public void DestroySelf()
    {
        health = 0;
        onTakeDamageEvent?.Invoke(0, health);
        onDestroyEvent?.Invoke();
        gameObject.SetActive(false);
        NavMeshMain.Instance.updateMesh();
        Destroy(gameObject);
    }
}
