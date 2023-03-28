using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MitosisEnemy : EnemySheep
{
    [SerializeField] private EnemySheepTypeSO standardEnemySheepSO;
    protected override void OnDeath()
    {
        Transform t;
        t = Instantiate(standardEnemySheepSO.prefab,transform.position +transform.right * 0.2f , Quaternion.identity);
        t.parent = transform.parent;
        t.GetComponent<SetTargetSheep>().setTarget(setTargetSheep.getTarget());
        t =Instantiate(standardEnemySheepSO.prefab, transform.position - transform.right * 0.2f, Quaternion.identity);
        t.parent = transform.parent;
        t.GetComponent<SetTargetSheep>().setTarget(setTargetSheep.getTarget());
        base.OnDeath();
    }
}
