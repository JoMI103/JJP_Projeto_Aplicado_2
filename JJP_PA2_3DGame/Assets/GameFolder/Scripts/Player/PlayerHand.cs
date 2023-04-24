using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] ToolSelector toolSelector;
    [SerializeField] PlayerBuild playerBuild;
    [SerializeField] SelectionInfo selectionInfo;

    private ItemSO activeItem;
    public ItemSO changeItem;

    private void Update()
    {
        if (activeItem != changeItem) updateTool();
    }


    private void updateTool()
    {
        activeItem = changeItem;

        if (activeItem.itemType == ItemSO.ItemType.bluePrint)
        {
            //Set bluePrint FUture
            playerBuild.setBuildingTypeSO(activeItem.building);

        }
        else
        {
            playerBuild.DeselectObjectType();
        }

        selectionInfo.generateItemInfo(activeItem);
        toolSelector.selectTool(activeItem.handId);
    }
}
