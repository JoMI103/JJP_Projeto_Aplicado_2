using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudTrap : Trap
{
    [Space(10)]
    [Header("MudTrapAtributes")]
    [Space(10)]

    public float slowEffect;
    public float effectTime;


    protected override void Start()
    {
        base.Start();
        StartCoroutine(slow());
    }

    private IEnumerator slow()
    {
        while (true)
        {
            GetSurroundSheeps();
            foreach (EnemySheep es in sheeps)
            {
                if (!es.slowEffectIsRunning || es.slow > slowEffect)  {
                    if(es.currentSlowEffect != null)
                    StopCoroutine(es.currentSlowEffect); 
                    es.currentSlowEffect = es.slowEffect(effectTime,slowEffect);  
                    es.startCurrentSlowEffect();
                }
            }
            yield return null;
        }
    }
}
