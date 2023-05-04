using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : Trap
{
    [Space(10)]
    [Header("SpikeTrapAtributes")]
    [Space(10)]

    public bool ok;


    protected override void Start()
    {
        base.Start();
        animator.speed =  aps;
    }

    public void attack()
    {
        au.Play("SpikeTrap");
        GetSurroundSheeps();
        foreach (EnemySheep es in sheeps) es?.receiveDmg(damage);  
    }
}