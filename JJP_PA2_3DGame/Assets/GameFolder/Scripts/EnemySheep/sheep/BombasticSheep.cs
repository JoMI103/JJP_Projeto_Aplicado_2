using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombasticSheep : EnemySheep
{
    [Space(10)]
    [Header("Bombastic Sheep Atributes")]
    [Space(10)]

    [SerializeField] private GameObject explosion;
 

    protected override void MoveAnim()
    {
        animator.Play("Walk");
    }

    protected override void AttackAndAtackAnim()
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
