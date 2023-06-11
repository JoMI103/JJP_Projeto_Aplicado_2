using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStats : MonoBehaviour
{
    private bool debugStatsChanges;

    

    private int HPPoints;
    public int hPPoints { get { return this.HPPoints; } set { this.HPPoints = value; updateHpUI(); } }
    private int WoodQuantity;
    public int woodQuantity{ get { return this.WoodQuantity; } set { this.WoodQuantity = value; updateWoodUI(); } }
    private int MetalQuantity;
    public int metalQuantity{ get { return this.MetalQuantity; } set { this.MetalQuantity = value; updateMetalUI(); } }
    private int EletronicsQuantity;
    public int eletronicsQuantity { get { return this.EletronicsQuantity; } set { this.EletronicsQuantity = value; updateEletronicsUI(); } }

     private Vector3 startPosition;

    private CharacterController characterController;

    private void Start()
    {
        woodQuantity =23000; //10000
        metalQuantity = 18000; //10000
        eletronicsQuantity = 500; //500
        hPPoints = maxHP;
         startPosition = transform.position;
         characterController = GetComponent<CharacterController>();
    }


    private const int maxHP = 5000;
    private const float starHealing = 5;
    float regTimer;

   
    
   

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) woodQuantity = woodQuantity + 1000;
        if (Input.GetKeyDown(KeyCode.K)) metalQuantity = metalQuantity + 1000;
        if (Input.GetKeyDown(KeyCode.I)) eletronicsQuantity = eletronicsQuantity + 100;
         if (Input.GetKeyDown(KeyCode.M)) giveDmg(10);
         
        if(hPPoints < maxHP){
            if(regTimer > starHealing){
                hPPoints += 50;
                if(hPPoints > maxHP) hPPoints = maxHP;
                regTimer-=0.5f;
            }
            regTimer += Time.deltaTime;
        }

        if(hPPoints < 1){
            characterController.enabled = false;
            Debug.LogError("DEAD");
            transform.position = startPosition;
            hPPoints = maxHP;
        }else   characterController.enabled = true ;
        
        if(Input.GetKeyDown(KeyCode.B)) {
             characterController.enabled = false;
              transform.position = startPosition;
        }
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

    public void giveDmg(int dmg){
        regTimer = 0;
        hPPoints-=dmg;
    }


    [SerializeField] Blit blit;
    private void updateHpUI()
    {
        blit.setHP(0.5f-((float)hPPoints / (float)maxHP) /2);
        hp.text = "" + HPPoints;
    }


   [SerializeField] private TextMeshProUGUI totalWood, totalMetal, totalEletronics;

    private void updateWoodUI()
    {
        totalWood.text = "" + (float)WoodQuantity / 1000 + "K";
    }

    private void updateMetalUI()
    {
        totalMetal.text = "" + (float)MetalQuantity / 1000 + "K";
    }

    private void updateEletronicsUI()
    {
        totalEletronics.text = "" + (float)EletronicsQuantity;
    }


}
