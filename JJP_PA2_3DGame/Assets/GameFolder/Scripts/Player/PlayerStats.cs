using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStats : MonoBehaviour
{
    private bool debugStatsChanges;

    private const int maxHP = 100;

    private int HPPoints;
    public int hPPoints { get { return this.HPPoints; } set { this.HPPoints = value; updateHpUI(); } }
    private int WoodQuantity;
    public int woodQuantity{ get { return this.WoodQuantity; } set { this.WoodQuantity = value; updateWoodUI(); } }
    private int MetalQuantity;
    public int metalQuantity{ get { return this.MetalQuantity; } set { this.MetalQuantity = value; updateMetalUI(); } }
    private int EletronicsQuantity;
    public int eletronicsQuantity { get { return this.EletronicsQuantity; } set { this.EletronicsQuantity = value; updateEletronicsUI(); } }


    private void Start()
    {
        woodQuantity = 10000;
        metalQuantity = 5000;
        eletronicsQuantity = 100;
        hPPoints = maxHP;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) woodQuantity = woodQuantity + 1000;
        if (Input.GetKeyDown(KeyCode.K)) metalQuantity = metalQuantity + 1000;
        if (Input.GetKeyDown(KeyCode.L)) eletronicsQuantity = eletronicsQuantity + 100;

    }


    public bool checkResourcesQuantity(BuildingTypeSO building)
    {
        if (WoodQuantity < building.woodCost || metalQuantity < building.metalCost || eletronicsQuantity < building.eletronicsCost) return false;
        return true;
    }


    public void useResources(BuildingTypeSO building)
    {
        woodQuantity -= building.woodCost;
        metalQuantity -= building.metalCost;
        eletronicsQuantity -= building.eletronicsCost;
    }

    //UI

    [SerializeField] private TextMeshProUGUI hp;

    private void updateHpUI()
    {
        hp.text = "HP: " + HPPoints;
    }


   [SerializeField] private TextMeshProUGUI totalWood, totalMetal, totalEletronics;

    private void updateWoodUI()
    {
        totalWood.text = "Wood: " + (float)WoodQuantity / 1000 + "K";
    }

    private void updateMetalUI()
    {
        totalMetal.text = "Metal: " + (float)MetalQuantity / 1000 + "K";
    }

    private void updateEletronicsUI()
    {
        totalEletronics.text = "Eletronics: " + (float)EletronicsQuantity / 1000 + "K";
    }


}
