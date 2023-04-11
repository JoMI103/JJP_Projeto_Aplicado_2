using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSheep : EnemySheep
{


    void Update()
    {
       base.Update();
    }

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
