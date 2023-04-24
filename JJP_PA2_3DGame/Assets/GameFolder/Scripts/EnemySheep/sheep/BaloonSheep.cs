using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaloonSheep : EnemySheep
{
    [Space(10)]
    [Header("Baloon Sheep Atributes")]
    [Space(10)]

    public bool nothing;

    protected override void MoveAnim()
    {
        animator.Play("Walk");
    }
    protected override void Attack()
    {

        placedBuilding.takeDamge(attackDmg);
    }
}
