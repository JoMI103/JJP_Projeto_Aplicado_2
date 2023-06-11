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

    [SerializeField] GameObject Principal, animationModel;
    [SerializeField] DefenseBuilding building;
    [SerializeField] MeshRenderer meshRenderer;

    private void Start() {
        SetStats();
        StartCoroutine("constroi");
    }
    private void SetStats()
    {
        health = buildingTypeSO.health;
        baseHealth = health;
    }

   

    public IEnumerator constroi(){
        float time = 0;
        while(time < 5f){
            meshRenderer.sharedMaterial.SetFloat("_Generate",time / 5);
            time += Time.deltaTime;
            yield return null;
        }
        
        Principal.SetActive(true);
        animationModel.SetActive(false);
        building.enabled = true;
    }


    public BuildingTypeSO buildingTypeSO;
    private Vector2Int origin;
    public BuildingTypeSO.Dir dir;

    public Transform CenterPosition;

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
