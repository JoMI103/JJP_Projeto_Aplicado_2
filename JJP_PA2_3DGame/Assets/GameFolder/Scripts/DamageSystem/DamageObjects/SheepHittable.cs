using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepHittable : Hittable
{
    private EnemySheep enemySheep;

    private void Awake()
    {
        enemySheep = GetComponent<EnemySheep>();
    }

    protected override void GiveDamage(int dmgValue)
    {
        enemySheep.receiveDmg(dmgValue);
    }
}
