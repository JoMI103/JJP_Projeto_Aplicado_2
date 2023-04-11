using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombasticSheep : EnemySheep
{
    [SerializeField] private GameObject explosion;
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
        Instantiate(explosion, transform.position, transform.rotation).GetComponent<SheepExplosion>().Explode(attackDmg);
        deathWithNoEffect();
    }

    protected override void OnDeath()
    {
        Instantiate(explosion, transform.position, transform.rotation).GetComponent<SheepExplosion>().Explode(attackDmg);
        base.OnDeath();
    }
}
