using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryGun : Tower
{
    [Space(10)] [Header("SentryGun")] [Space(10)]
    
    [SerializeField] private Transform shotPoint, pivotPoint;
    
    
    public void Update()
    {
        if (targetSheep == null) return;
        targetPos = targetSheep.transform.position;

        Vector3 toPosition = (new Vector3(targetPos.x, pivotPoint.position.y, targetPos.z) - pivotPoint.position).normalized;
        float angle = SignedAngleBetween( Vector3.forward,  toPosition, Vector3.up);
        pivotPoint.rotation = Quaternion.Euler(0, angle, 0);
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
    
    protected override  IEnumerator Scan(){
        WaitForSeconds waitForNextScan = new WaitForSeconds(1f);

        while (true)
        {
            yield return aps;
            GetSurroundSheeps();
           
            if (sheeps.Count == 0) { yield return waitForNextScan; }//Debug.Log("Waiting for next scan"); 
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
                
                float timeFocus = aps * 6;
                while(targetSheep != null && timeFocus > 0){
                     shootAnimation();
                     timeFocus -= aps;
                     Debug.Log("time" +  timeFocus);
                     yield return new WaitForSeconds( aps);
                }
                
            }

        }
    }
    
    
    protected override void shootAnimation()
    {
        targetSheep.receiveDmg(damage);
        //targetPos = targetSheep.getFuturePoint(5, calculateTime(targetSheep.transform.position) + 0.4f);
       // animator.Play(attackAnimationName);
    }
    
    
        #region gizmos

    public bool debugScanArea;

    private void OnDrawGizmos()
    {
        if (!debugScanArea) return;

        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireSphere(pivotPoint.position, innerScanRadius);
        Gizmos.color = UnityEngine.Color.green;
        Gizmos.DrawWireSphere(pivotPoint.position, outScanRadius);
    }

    #endregion

}