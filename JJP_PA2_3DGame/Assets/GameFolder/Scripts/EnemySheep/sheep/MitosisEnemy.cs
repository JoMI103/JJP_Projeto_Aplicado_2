using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MitosisEnemy : EnemySheep
{
    [SerializeField] private EnemySheepTypeSO standardEnemySheepSO;
    protected override void OnDeath()
    {
        Transform SheepTransform;
        EnemySheep SheepCode;
        SheepTransform = Instantiate(standardEnemySheepSO.prefab, transform.position + transform.right * 0.2f, transform.rotation);
        SheepTransform.parent = transform.parent;
        SheepTransform.GetComponent<EnemySheep>().setPlayerAndObjective(playerPosition, ObjectivePosition);

        SheepTransform = Instantiate(standardEnemySheepSO.prefab, transform.position - transform.right * 0.2f, transform.rotation);
        SheepTransform.parent = transform.parent;
        SheepTransform.GetComponent<EnemySheep>().setPlayerAndObjective(playerPosition, ObjectivePosition);

        base.OnDeath();

    }
}
