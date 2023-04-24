using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardSheep : EnemySheep
{
    [Space(10)]
    [Header("Standard Sheep Atributes")]
    [Space(10)]

    public bool nothing;





    protected override void MoveAnim()
    {
        animator.Play("Walk");
    }

    protected override void Attack()
    {
        //animator.Play("AttackBaseSheep");
        placedBuilding.takeDamge(attackDmg);
    }
}
