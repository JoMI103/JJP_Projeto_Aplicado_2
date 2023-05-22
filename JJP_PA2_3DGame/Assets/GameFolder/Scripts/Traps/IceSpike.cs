using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpike : Tower
{
     [Space(10)]
    [Header("IceSpikeAtributes")]
    [Space(10)]
    
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] float stunDuration;    
    
    protected override void shootAnimation()
    {
        foreach(EnemySheep es in sheeps){
            
            es.receiveDmg(damage);
            if (!es.wasStunned)  {
                if(es.currentStunEffect != null) StopCoroutine(es.currentStunEffect); 
                es.currentStunEffect = es.stunEffect(stunDuration,aps);  
                es.startCurrentStunEffect();
            }
            
        }
        
        StartCoroutine(explosion());
        //animator.Play(attackAnimationName);
    }
    
    private IEnumerator explosion(){
        float cRadius = 0;
        
        while(cRadius < outScanRadius){
            meshRenderer.sharedMaterial.SetFloat("_ExplosionTime",cRadius);
            cRadius += Time.deltaTime * 30;
             yield return null;
        }
         yield return null;
       
        meshRenderer.sharedMaterial.SetFloat("_ExplosionTime",0);
    }
    
    protected override IEnumerator Scan()
    {
        WaitForSeconds waitAps = new WaitForSeconds(aps);
        WaitForSeconds waitForNextScan = new WaitForSeconds(0.5f);

        while (true)
        {
            GetSurroundSheeps();

            if (sheeps.Count == 0) { yield return waitForNextScan; }
            else
            {
             
                    shootAnimation();
                    yield return waitAps;
         
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
    }
    
    #region gizmos

    public bool debugScanArea;

    private void OnDrawGizmos()
    {
        if (!debugScanArea) return;

        Gizmos.color = UnityEngine.Color.green;
        Gizmos.DrawWireSphere(centerPoint.position, outScanRadius);
    }

    #endregion
}
