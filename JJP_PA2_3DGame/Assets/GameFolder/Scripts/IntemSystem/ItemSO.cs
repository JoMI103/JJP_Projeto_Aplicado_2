using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "SO/Item")]
public class ItemSO : ScriptableObject
{
    public string itemNameString;
    public Sprite image;

    public int handId;
    public bool bluePrint;
    public BuildingTypeSO building;

}