using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSheep : EnemySheep
{
    [Space(10)]
    [Header("Boss Sheep Atributes")]
    [Space(10)]

    public bool nothing;

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
