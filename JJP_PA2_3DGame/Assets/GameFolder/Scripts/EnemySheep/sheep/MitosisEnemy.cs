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
        EnemySheep es;
        SheepTransform = Instantiate(standardEnemySheepSO.prefab, transform.position + transform.right * 0.2f+ transform.up, transform.rotation);
        SheepTransform.parent = transform.parent;
        es =  SheepTransform.GetComponent<EnemySheep>();
        es.setPlayerAndObjective(playerPosition, ObjectivePosition);
        es.startknockBackEffect(-transform.right+transform.up,5f,true);
        
        
        SheepTransform = Instantiate(standardEnemySheepSO.prefab, transform.position - transform.right * 0.2f+ transform.up, transform.rotation);
        SheepTransform.parent = transform.parent;
         es =  SheepTransform.GetComponent<EnemySheep>();
        es.setPlayerAndObjective(playerPosition, ObjectivePosition);
        es.startknockBackEffect(transform.right+transform.up,5f,true);
        base.OnDeath();

    }

    protected override void AttackAndAtackAnim()
    {
        animator.Play("Attack");
        base.addResources();
        targetedBuilding.takeDamge(sheepAttackDmg);
    }
}
