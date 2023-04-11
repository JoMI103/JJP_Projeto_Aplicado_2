using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantBoxingGlove : Trap
{
    [Space(10)]
    [Header("GiantBoxingGloveAtributes")]
    [Space(10)]

    public bool ok;

    protected override void Start()
    {
        base.Start();
        animator.speed = 1 / aps;
    }

    public void attack()
    {
        //au.Play("SpikeTrap");
        GetSurroundSheeps();
        foreach (EnemySheep es in sheeps) { es?.receiveDmg(damage); ; }
    }
}
