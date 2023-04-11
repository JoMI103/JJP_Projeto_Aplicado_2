using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DefenseBuilding : MonoBehaviour
{    
    [Header("DefenseBuildingAtributes")]
    [Space(10)]

    [SerializeField] protected Transform centerPoint;
    [SerializeField] protected float innerScanRadius, outScanRadius;

    [SerializeField] protected LayerMask sheepLayer;
    [SerializeField] protected List<EnemySheep> sheeps;

    protected AudioManager au;
    protected PlacedBuilding placedBuilding;
    protected Animator animator;
    //Stats
    protected int damage;
    protected float aps;

    private void Awake()
    {
        placedBuilding = GetComponent<PlacedBuilding>();
        au = GetComponent<AudioManager>();
        sheeps = new List<EnemySheep>();
        animator = GetComponent<Animator>();    


    }

    protected virtual void Start() 
    {
        damage = placedBuilding.buildingTypeSO.damage;
        aps = placedBuilding.buildingTypeSO.aps;
        if (aps == 0) aps = 1f;
    }
    protected virtual void GetSurroundSheeps() { }
}
