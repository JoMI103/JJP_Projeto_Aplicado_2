using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    [SerializeField] private BuildingTypeSO building;
    [SerializeField] private Transform rotatePoint;
    [SerializeField] private float Velocity;
    
    
    private void Update() {
        transform.RotateAround(transform.position,Vector3.up, Time.deltaTime * Velocity);
    }
    
    [ContextMenu("UpdateSheep")]
    public void updateSheep(){
        for(int i = (this.transform.childCount -1); i >= 0 ; i--){
            Transform t =  this.transform.GetChild(i); DestroyImmediate(t.gameObject);
        }
        
        Transform buildingT = Instantiate(building.prefab, transform.position,Quaternion.identity);
        buildingT.parent = this.transform;
        buildingT.position = new Vector3(-building.width ,0,-building.width ); 
        
    }
    
}
