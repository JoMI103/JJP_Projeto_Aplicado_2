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
/*
    private int HPPoints { get { return HPPoints; } set { HPPoints = value; updateHpUI(); } }
    private int WoodQuantity { get { return WoodQuantity; } set { WoodQuantity = value; updateWoodUI(); } }
    private int MetalQuantity { get { return MetalQuantity; } set { MetalQuantity = value; updateMetalUI(); } }
    private int EletronicsQuantity { get { return EletronicsQuantity; } set { EletronicsQuantity = value; updateEletronicsUI(); } }


    private void Start()
    {
        WoodQuantity = 0;
        MetalQuantity = 0;
        EletronicsQuantity = 0;
        HPPoints = maxHP;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) WoodQuantity = WoodQuantity + 1000;
        if (Input.GetKeyDown(KeyCode.K)) MetalQuantity = MetalQuantity + 1000;
        if (Input.GetKeyDown(KeyCode.L)) EletronicsQuantity = EletronicsQuantity + 100;

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
        totalWood.text = "Wood: " + WoodQuantity / 1000 + "K";
    }

    private void updateMetalUI()
    {
        totalMetal.text = "Metal: " + MetalQuantity / 1000 + "K";
    }

    private void updateEletronicsUI()
    {
        totalEletronics.text = "Eletronics: " + EletronicsQuantity / 1000 + "K";
    }
*/
}
