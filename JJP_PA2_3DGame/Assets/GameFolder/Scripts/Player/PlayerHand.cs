using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] ToolSelector toolSelector;
    [SerializeField] PlayerBuild playerBuild; GunSystem currentGunSystem; public toolSystem toolsystem;
    [SerializeField] SelectionInfo selectionInfo;

    public ItemSO activeItem;
    public ItemSO changeItem;

    private InputManager inputManager;
    
    [SerializeField] ItemSO startItem;
    
    void Start() {
        selectTool(startItem);
        inputManager = GetComponent<InputManager>();

   
        inputManager.onFoot.RotateReload.performed += ctx => tryRotateReload();
        inputManager.onFoot.Destroy.performed += ctx => tryDestroy();
    }

    private void Update() {
        if (activeItem != changeItem) updateTool();
         if ( inputManager.onFoot.PlaceShootAttack.inProgress){ tryPlaceShootAttack();}
    }
    
    bool buildCooldDown = true, shootCoolDown = true;
    private void shootTrue(){shootCoolDown = true;}
    private void buildTrue(){buildCooldDown = true;}

    private void tryPlaceShootAttack(){
        switch (activeItem.itemType) {
            case ItemSO.ItemType.HandTool:
            if(toolsystem != null &&!toolsystem.isInAction)
                toolsystem.toolAction();
            break;
            case ItemSO.ItemType.Gun:
            if(shootCoolDown){
                currentGunSystem.tryShoot();
                shootCoolDown = false;
                Invoke("shootTrue",0.5f);
            }
                break;
            case ItemSO.ItemType.bluePrint: 
                if(buildCooldDown){
            
                playerBuild.PlaceBuilding();
                buildCooldDown = false;
                Invoke("buildTrue",0.2f);
            }
            break;
        }
    }
    
    private void tryRotateReload(){
        switch (activeItem.itemType) {
            case ItemSO.ItemType.HandTool: break;
            case ItemSO.ItemType.Gun:
                currentGunSystem.tryReload();
                break;
            case ItemSO.ItemType.bluePrint:
                playerBuild.RotateBuilding();
             break;
        }
    }
    private void tryDestroy(){
        switch (activeItem.itemType) {
            case ItemSO.ItemType.HandTool: break;
            case ItemSO.ItemType.Gun: break;
            case ItemSO.ItemType.bluePrint:
                playerBuild.DestroyBuilding();
             break;
        }
    }


    private void updateTool()
    {
        activeItem = changeItem;

        if (activeItem.itemType == ItemSO.ItemType.bluePrint) 
            playerBuild.setBuildingTypeSO(activeItem.building);
        else
            playerBuild.DeselectObjectType();
        

        selectionInfo.generateItemInfo(activeItem);
        selectTool(activeItem);
    }

    [SerializeField] GameObject[] tools;


   
    public void selectTool(ItemSO item) {
        deSelectAll();
        
        if(item.handId >= 0 && item.handId < tools.Length){
            switch (item.itemType) {
                case ItemSO.ItemType.HandTool: 
                 toolsystem = tools[item.handId].GetComponent<toolSystem>();
                 break;
                   
                case ItemSO.ItemType.Gun:
                    currentGunSystem = tools[item.handId].GetComponent<GunSystem>();
                    break;
                case ItemSO.ItemType.bluePrint: break;
            }
            
            tools?[item.handId].SetActive(true);
        }
    }

    private void deSelectAll() {
        foreach (GameObject tool in tools)
            if (tool != null) tool.SetActive(false);
    }

}
