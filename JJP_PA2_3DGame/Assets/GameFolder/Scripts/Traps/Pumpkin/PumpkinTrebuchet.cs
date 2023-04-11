using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinTrebuchet : Tower
{
    [Space(10)] [Header("PumpkinTrebuchetAtributes")] [Space(10)]
    [SerializeField] private Transform pumpkimPrefab;
    [SerializeField] private Transform shotPoint, pivotPoint;
    [SerializeField] private float gravity, h;
    public bool debugPath;

    
    [SerializeField] private float explosionRadius;

    public void Update()
    {
        if (targetPos == null) return;
        pivotPoint.transform.LookAt(new Vector3(targetPos.x,pivotPoint.position.y, targetPos.z));
        if (debugPath) { DrawPath(); }
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