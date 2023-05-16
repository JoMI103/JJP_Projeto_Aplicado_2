using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BombasticSheep : EnemySheep
{
    [Space(10)]
    [Header("Bombastic Sheep Atributes")]
    [Space(10)]

    [SerializeField] private GameObject explosion;
 
    
    override protected void Start() {
     
       NavMeshQueryFilter navMeshQueryFilter = new NavMeshQueryFilter();
            navMeshQueryFilter.areaMask = NavMesh.AllAreas;
            navMeshAgent.SetAreaCost(NavMesh.GetAreaFromName("Wall"),2);
        Debug.Log("ok");
        base.Start();
    }
    
    protected override void MoveAnim()
    {
        animator.Play("Walk");
    }

    protected override void AttackAndAtackAnim()
    {
        Instantiate(explosion, transform.position, transform.rotation).GetComponent<SheepExplosion>().Explode(sheepAttackDmg);
        deathWithNoEffect();
    }

    protected override void OnDeath()
    {
        Instantiate(explosion, transform.position, transform.rotation).GetComponent<SheepExplosion>().Explode(sheepAttackDmg);
        base.OnDeath();
    }
}
