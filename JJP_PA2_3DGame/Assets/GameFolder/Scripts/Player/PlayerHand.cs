using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] ToolSelector toolSelector;
    [SerializeField] PlayerBuild playerBuild;

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
            playerBuild.setBuildingTypeSO(activeItem.building);

        }
        else
        {
            playerBuild.DeselectObjectType();
        }
        

        toolSelector.selectTool(activeItem.handId);
    }
}
