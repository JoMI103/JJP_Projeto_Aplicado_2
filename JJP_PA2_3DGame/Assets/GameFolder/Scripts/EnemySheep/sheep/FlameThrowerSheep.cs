using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerSheep : EnemySheep
{
    [Space(10)]
    [Header("FlameThrower Sheep Atributes")]
    [Space(10)]

    public bool nothing;
    [SerializeField] private GameObject flameParticles;

    protected override void MoveAnim()
    {
        animator.Play("Walk");
    }

    protected override void whenTargetDestroy()
    {
        flameParticles.SetActive(false);
        changeCurrentState(state.FollowPath);
    }
    protected override void Attack()
    {
        flameParticles.SetActive(true);
        placedBuilding.takeDamge(attackDmg);
    }
}
