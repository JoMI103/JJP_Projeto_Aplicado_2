using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardSheep : EnemySheep
{
    [Space(10)]
    [Header("Standard Sheep Atributes")]
    [Space(10)]

    public bool nothing;

    public Transform p,o;
    protected override void Start()
    {
        if(p != null && o != null)
        setPlayerAndObjective(p,o);
        base.Start();
    }



    protected override void MoveAnim()
    {
        animator.Play("Walk");
    }

    protected override void AttackAndAtackAnim()
    {
        animator.Play("Attack");
        targetedBuilding.takeDamge(sheepAttackDmg);
    }
}
