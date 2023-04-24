using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "SO/Item")]
public class ItemSO : ScriptableObject
{
    public enum ItemType  {bluePrint, HandTool, Gun}

    public string itemNameString;
    public Sprite image;

    public int handId;
    public ItemType itemType;
    public BuildingTypeSO building;

}
