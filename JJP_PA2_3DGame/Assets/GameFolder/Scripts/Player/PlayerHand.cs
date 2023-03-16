using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] ToolSelector toolSelector;
    [SerializeField] GridBuildingSystem gridBuildingSystem;

    private ItemSO activeItem;
    public ItemSO changeItem;

    private void Update()
    {
        if (activeItem != changeItem) updateTool();
    }


    private void updateTool()
    {
        activeItem = changeItem;

        if (activeItem.bluePrint)
        {
            //Set bluePrint FUture
            gridBuildingSystem.setBuildingTypeSO(activeItem.building);

        }
        else
        {
            gridBuildingSystem.DeselectObjectType();
        }
        

        toolSelector.selectTool(activeItem.handId);
    }
}
