using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantBoxingGlove : Trap
{
    [Space(10)]
    [Header("GiantBoxingGloveAtributes")]
    [Space(10)]

    public bool ok;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        while (true)
        {
            animator.Play("OnePunchMan");
            yield return new WaitForSeconds(2);
        }
    }



    public void attack()
    {
        //au.Play("SpikeTrap");
        GetSurroundSheeps();
        foreach (EnemySheep es in sheeps) { es?.receiveDmg(damage); es?.startknockBackEffect(centerPoint.up*10 +  (es.transform.position- this.transform.position).normalized * 0.3f, 2f); }
    }
}
