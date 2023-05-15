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

    protected override void AttackAndAtackAnim()
    {
        animator.Play("Attack");
         Debug.LogError( "Atacking" + targetedBuilding.name);
        targetedBuilding.takeDamge(sheepAttackDmg);
    }
}
