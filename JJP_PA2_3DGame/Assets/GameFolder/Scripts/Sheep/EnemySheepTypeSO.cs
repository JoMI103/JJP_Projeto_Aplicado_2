using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class EnemySheepTypeSO : ScriptableObject
{
    public string nameString;
    public string description;
    public int baseHealth;
    public int baseDmg;
    public Transform prefab;
}
