using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MitosisEnemy : EnemySheep
{
    [SerializeField] private GameObject standardEnemySheep;
    protected override void OnDeath()
    {

        Debug.Log("hi");
        Instantiate(standardEnemySheep, new Vector3(this.transform.position.x+0.2f, this.transform.position.y, this.transform.position.z), Quaternion.identity);
        Instantiate(standardEnemySheep, new Vector3(this.transform.position.x-0.2f, this.transform.position.y, this.transform.position.z), Quaternion.identity);

        base.OnDeath();
    }
}
