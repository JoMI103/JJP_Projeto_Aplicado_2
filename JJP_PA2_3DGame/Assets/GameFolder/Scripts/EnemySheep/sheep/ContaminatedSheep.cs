using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContaminatedSheep : EnemySheep
{
    [Space(10)]
    [Header("Contaminated Sheep Atributes")]
    [Space(10)]

    public bool nothing;

    protected override void MoveAnim()
    {
        animator.Play("Walk");
    }
    protected override void AttackAndAtackAnim()
    {
        
        targetedBuilding.takeDamge(attackDmg);
    }
}
