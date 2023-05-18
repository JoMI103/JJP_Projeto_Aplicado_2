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

        inputManager.onFoot.PlaceShootAttack.performed += ctx => tryPlaceShootAttack();
        inputManager.onFoot.RotateReload.performed += ctx => tryRotateReload();
        inputManager.onFoot.Destroy.performed += ctx => tryDestroy();
    }

    private void Update() {
        if (activeItem != changeItem) updateTool();
    }

    private void tryPlaceShootAttack(){
        switch (activeItem.itemType) {
            case ItemSO.ItemType.HandTool:
                //CurrentToolAction
            break;
            case ItemSO.ItemType.Gun:
                currentGunSystem.tryShoot();
                break;
            case ItemSO.ItemType.bluePrint: 
                playerBuild.PlaceBuilding();
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
                case ItemSO.ItemType.HandTool: break;
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
