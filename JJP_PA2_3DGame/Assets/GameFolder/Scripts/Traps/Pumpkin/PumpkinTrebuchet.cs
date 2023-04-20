using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PumpkinTrebuchet : Tower
{
    [Space(10)] [Header("PumpkinTrebuchetAtributes")] [Space(10)]
    [SerializeField] private Transform pumpkimPrefab;
    [SerializeField] private Transform shotPoint, pivotPoint;
    [SerializeField] private float gravity, h;
    public bool debugPath;

    
    [SerializeField] private float explosionRadius;

    [SerializeField] private Transform playerfordebug;

    public void Start()
    {
        
/*          
        pivotPoint.parent.localRotation = ;
            //Quaternion.Euler(0f,placedBuilding.buildingTypeSO.GetCenterRotationAngle(placedBuilding.dir), 0f);
        Debug.Log(pivotPoint.rotation);
  */
    }


    public void Update()
    {
        //if (targetPos == null) return;

        Vector3 toPosition = (new Vector3(playerfordebug.position.x, pivotPoint.position.y, playerfordebug.position.z) - pivotPoint.position).normalized;
        // Quaternion rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);


        //pivotPoint.LookAt(playerfordebug, pivotPoint.parent.up);

        Debug.Log(toPosition)
;
        float angle = SignedAngleBetween( Vector3.forward,  toPosition, Vector3.up);
        
 

        pivotPoint.rotation = Quaternion.Euler(0, angle, 0);
        pivotPoint.localRotation = Quaternion.Euler(0, pivotPoint.localRotation.eulerAngles.y, 0);

       // pivotPoint.rotation = transform.parent.worldToLocalMatrix.rotation *  pivotPoint.rotation;

        if (debugPath) { DrawPath(); }
    }


    public static float CalculateAngle(Vector3 from, Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.forward, to - from).eulerAngles.z;
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

    protected override void shoot()
    {

        targetPos = targetSheep.getFuturePoint(5, 3);

        LaunchData launchData = calculateLaunchData();
        if (!checkPath(launchData)) return;
        Transform pumpkimProjectile = Instantiate(pumpkimPrefab, shotPoint.position, shotPoint.rotation);
        pumpkimProjectile.GetComponent<Rigidbody>().velocity = launchData.initialVelocity;
        pumpkimProjectile.GetComponent<PumpkinImpact>().SetExplosionStats(placedBuilding.buildingTypeSO.damage);
    }
 

    #region launchPathData

    LaunchData calculateLaunchData()
    {
        float displacementY = targetPos.y - shotPoint.position.y;
        Vector3 displacementXZ = new Vector3(targetPos.x - shotPoint.position.x, 0, targetPos.z - shotPoint.position.z);
        float time = MathF.Sqrt(-2 * h / gravity) + MathF.Sqrt(2 * (displacementY - h) / gravity); 
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * - Mathf.Sign(gravity), time) ;
    }

    [SerializeField]  LayerMask projectileIgnore;

    private bool checkPath(LaunchData launchData)
    {
        Vector3 previousDrawPoint = shotPoint.position;

        int resolution = 30;
        for (int i = 1; i < resolution; i++)
        {
            float simulationTime = i / (float)resolution * launchData.timeTotarget;
            Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
            Vector3 drawPoint = shotPoint.position + displacement;

            Vector3 direction = (previousDrawPoint - drawPoint).normalized;

            Debug.DrawLine(previousDrawPoint, drawPoint, Color.blue);

            if (Physics.Raycast(previousDrawPoint, direction, Vector3.Distance(previousDrawPoint, drawPoint), projectileIgnore))
            {
                Debug.Log("____________");
                Debug.Log(i);
                Debug.Log("FALSO");

                return false;

            }
            previousDrawPoint = drawPoint;
        }
        return true;

    }

    private void DrawPath()
    {
        LaunchData launchData = calculateLaunchData();
        Vector3 previousDrawPoint = shotPoint.position;

        int resolution = 30;
        for (int i = 1; i < resolution; i++)
        {
            float simulationTime = i / (float)resolution * launchData.timeTotarget;
            Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
            Vector3 drawPoint = shotPoint.position + displacement;

           // Debug.DrawLine(previousDrawPoint, drawPoint, Color.red);
            previousDrawPoint = drawPoint;
        }
    }

    struct LaunchData {
        public readonly Vector3 initialVelocity;
        public readonly float timeTotarget;

        public LaunchData(Vector3 initialVelocity, float timeTotarget) {
            this.initialVelocity = initialVelocity;
            this.timeTotarget= timeTotarget;
        }
    }

    #endregion

    #region gizmos

    public bool debugScanArea;

    private void OnDrawGizmos()
    {
        if (!debugScanArea) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pivotPoint.position, innerScanRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pivotPoint.position, outScanRadius);
    }

    #endregion
}