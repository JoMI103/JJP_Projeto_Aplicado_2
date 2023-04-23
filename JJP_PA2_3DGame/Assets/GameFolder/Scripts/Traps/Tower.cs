using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : DefenseBuilding
{
    [Space(10)] [Header("TowerAtributes")] [Space(10)]

    [SerializeField] protected attackMode currentAttackMode;
    public enum attackMode { closest, First, Last, Strongest }

    protected EnemySheep targetSheep;
    protected Vector3 targetPos;


    protected override void Start() { base.Start(); StartCoroutine(Scan()); }

    protected WaitForSeconds nextAttack = new WaitForSeconds(1);

    protected virtual IEnumerator Scan()
    {
        WaitForSeconds waitForNextScan = new WaitForSeconds(0.5f);

        while (true)
        {
            GetSurroundSheeps();

            if (sheeps.Count == 0) { yield return waitForNextScan; Debug.Log("Waiting for next scan"); }
            else
            {
                switch (currentAttackMode)
                {
                    case attackMode.closest: attackClosest(); break;
                    case attackMode.First: attackFirst(); break;
                    case attackMode.Last: attackLast(); break;
                    case attackMode.Strongest: attackStrongest(); break;
                }

                if(targetSheep != null)
                {
                    shootAnimation();
                    yield return nextAttack;
                }else
                yield return waitForNextScan;
            }

        }
    }

    protected override void GetSurroundSheeps()
    {
        targetSheep = null;
        sheeps.Clear();

        var outSurroundingObjects = Physics.OverlapSphere(centerPoint.position, outScanRadius, sheepLayer);
        foreach (var surroundingObject in outSurroundingObjects)
        {
            EnemySheep enemySheep = surroundingObject.GetComponent<EnemySheep>();
            if (enemySheep != null) { sheeps.Add(enemySheep); }
        }

        if (sheeps.Count == 0) { return; }
        Debug.Log("BOm dia");

        List<EnemySheep> removeSheeps = new List<EnemySheep>();
       

        var innerSurroundingObjects = Physics.OverlapSphere(centerPoint.position, innerScanRadius, sheepLayer);
        foreach (var surroundingObject in innerSurroundingObjects)
        {
            EnemySheep enemySheep = surroundingObject.GetComponent<EnemySheep>();
            if (enemySheep != null) { removeSheeps.Add(enemySheep); }
        }

        foreach (EnemySheep es in removeSheeps) sheeps.Remove(es);
    }

    protected virtual void attackClosest()
    {

    }

    protected virtual void attackFirst()
    {
        while (sheeps.Count > 0)
        {
            if (sheeps[0] != null) { targetPos = sheeps[0].transform.position; targetSheep = sheeps[0]; break; }
            else sheeps.RemoveAt(0);
        }
    }
    
    protected virtual void attackLast()
    {
        while (sheeps.Count > 0)
        {
            if (sheeps[sheeps.Count - 1] != null) { targetPos = sheeps[sheeps.Count - 1].transform.position; break; }
            else sheeps.RemoveAt(sheeps.Count - 1);
        }
    }
   
    protected virtual void attackStrongest()
    {

    }
    

    protected virtual void shootAnimation() { }
    protected virtual void shoot() { }
}
