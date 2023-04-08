using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemySheep")]
public class EnemySheepTypeSO : ScriptableObject
{
    public string nameString;
    public string description;
    public int baseHealth;
    public int baseDmg;
    public float baseSpeed;
    public float AtackRange;
    public Transform prefab;
}
