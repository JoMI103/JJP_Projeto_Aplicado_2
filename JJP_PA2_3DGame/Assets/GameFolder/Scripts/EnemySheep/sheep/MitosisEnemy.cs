using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MitosisEnemy : EnemySheep
{
    [Space(10)]
    [Header("Mitosis Enemy Atributes")]
    [Space(10)]


    [SerializeField] private EnemySheepTypeSO standardEnemySheepSO;

    protected override void MoveAnim()
    {
        animator.Play("Walk");
    }
    protected override void OnDeath()
    {
        Transform SheepTransform;
 
        SheepTransform = Instantiate(standardEnemySheepSO.prefab, transform.position + transform.right * 0.2f, transform.rotation);
        SheepTransform.parent = transform.parent;
        SheepTransform.GetComponent<EnemySheep>().setPlayerAndObjective(playerPosition, ObjectivePosition);
        // SheepTransform.GetComponent<EnemySheep>().startknockBackEffect(-transform.right+transform.up,1f);
        SheepTransform = Instantiate(standardEnemySheepSO.prefab, transform.position - transform.right * 0.2f, transform.rotation);
        SheepTransform.parent = transform.parent;
        SheepTransform.GetComponent<EnemySheep>().setPlayerAndObjective(playerPosition, ObjectivePosition);
       // SheepTransform.GetComponent<EnemySheep>().startknockBackEffect(transform.right+transform.up,1f);
        base.OnDeath();

    }

    protected override void AttackAndAtackAnim()
    {
        animator.Play("Attack");
        targetedBuilding.takeDamge(sheepAttackDmg);
    }
}
