using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameTurret : Tower
{
    protected override void Start() { 
        base.Start();
        currentAngle = SignedAngleBetween( Vector3.forward,  transform.position + Vector3.forward, Vector3.up);
    }
    
    [SerializeField] private Transform shotPoint, pivotPoint;
    [SerializeField] private ParticleSystem flameParticles;
    float currentAngle; [SerializeField] float rotateVelocity;
    
    
     public void Update()
    {
        if (targetSheep == null) return;
        targetPos = targetSheep.transform.position;

        Vector3 toPosition = (new Vector3(targetPos.x, pivotPoint.position.y, targetPos.z) - pivotPoint.position).normalized;
        float angle = SignedAngleBetween( Vector3.forward,  toPosition, Vector3.up);
        currentAngle = Mathf.Lerp(currentAngle, angle, Time.deltaTime * rotateVelocity);
        pivotPoint.rotation = Quaternion.Euler(0, currentAngle, 0);
        pivotPoint.localRotation = Quaternion.Euler(0, pivotPoint.localRotation.eulerAngles.y, 0);

    }
    
     float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n)
    {
        // angle in [0,180]
        float angle = Vector3.Angle(a, b);
       
        float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));

        // angle in [-179,180]
        float signed_angle = angle * sign;

        // angle in [0,360] (not used but included here for completeness)
        //float angle360 =  (signed_angle + 180) % 360;

        return signed_angle;
    }
    
    protected override void shootAnimation()
    {
        flameParticles.gameObject.SetActive(true);
        flameParticles.Play(true);
     
        //animator.Play(attackAnimationName);
    }
    
    [SerializeField] private float timeFocus;
    
    protected override  IEnumerator Scan(){
        WaitForSeconds waitForNextScan = new WaitForSeconds(1f);

        while (true)
        {
            yield return null;
            GetSurroundSheeps();
           
            if (sheeps.Count == 0) { flameParticles.Stop(false); yield return waitForNextScan; }//Debug.Log("Waiting for next scan"); 
            else
            {
                switch (currentAttackMode)
                {
                    case attackMode.closest: attackClosest(); break;
                    case attackMode.First: attackFirst(); break;
                    case attackMode.Last: attackLast(); break;
                    case attackMode.Strongest: attackStrongest(); break;
                    case attackMode.random: attackRandom(); break;
                }
                
               
                if(targetSheep != null){
                    
                    shootAnimation();
                    float timeFiring = 0;
                    
                    
                    while(targetSheep != null && Vector3.Distance(transform.position, targetSheep.transform.position) < outScanRadius && timeFiring < timeFocus){
                        timeFiring += aps;
                        fire();
                        yield return new WaitForSeconds(aps);
                    }
                }
            }
        }
    }
    
    [SerializeField, Range(-1,1)] private float areaCone;
    
    private void fire(){


        var surroundingObjects = Physics.OverlapSphere(transform.position,outScanRadius, sheepLayer);

        foreach(var surroundingObject in surroundingObjects) {
            EnemySheep es = surroundingObject.GetComponent<EnemySheep>();
            if (es != null) { 
                float dotproduct = Vector3.Dot(centerPoint.forward,( es.transform.position-centerPoint.position).normalized);
                if(dotproduct > areaCone)  // area do cone
                es.receiveDmg(damage);
      
            }
          
        }     
    }
  
    #region gizmos

    public bool debugScanArea;

    private void OnDrawGizmos()
    {
        if (!debugScanArea) return;

        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireSphere(centerPoint.position, innerScanRadius);
        Gizmos.color = UnityEngine.Color.green;
        Gizmos.DrawWireSphere(centerPoint.position, outScanRadius);
    }

    #endregion
}

