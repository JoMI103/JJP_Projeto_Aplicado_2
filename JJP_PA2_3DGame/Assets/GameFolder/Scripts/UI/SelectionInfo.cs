using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectionInfo : MonoBehaviour
{
    [Header("Tool")]
    [SerializeField] GameObject toolInfo;
    [SerializeField] private TextMeshProUGUI toolName;
    [Header("Gun")]
    [SerializeField] GameObject gunInfo;
    [SerializeField] private TextMeshProUGUI gunName;
    [Header("BluePrint")]
    [SerializeField] GameObject bluePrintInfo;
    [SerializeField] private TextMeshProUGUI bluePrintName;
    [SerializeField] private TextMeshProUGUI woodCost, metalCost, EletronicsCost;




    public void generateItemInfo(ItemSO itemSO)
    {
        switch (itemSO.itemType)
        {
            case ItemSO.ItemType.HandTool:
                toolInfo.SetActive(true); gunInfo.SetActive(false); bluePrintInfo.SetActive(false);
                toolName.text = itemSO.itemNameString;
                break;
            case ItemSO.ItemType.Gun:
                toolInfo.SetActive(false); gunInfo.SetActive(true); bluePrintInfo.SetActive(false);
                gunName.text = itemSO.itemNameString;
                break;
            case ItemSO.ItemType.bluePrint:
                toolInfo.SetActive(false); gunInfo.SetActive(false); bluePrintInfo.SetActive(true);
                bluePrintName.text = itemSO.itemNameString;
                if(itemSO.building != null)
                {
                    woodCost.text = ": " + itemSO.building.woodCost.ToString();
                    metalCost.text = ": " + itemSO.building.metalCost.ToString();
                    EletronicsCost.text = ": " + itemSO.building.eletronicsCost.ToString();
                }

                break;
        }
    }

     

}
