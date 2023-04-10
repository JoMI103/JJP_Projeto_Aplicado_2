using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PumpkinTrebuchet : MonoBehaviour
{
    [SerializeField] private Transform pumpkimPrefab, shotPoint, pivotPoint;
    private Vector3 targetPos;
    private EnemySheep targetSheep;
    [SerializeField] private float gravity, h;
    [SerializeField] float blastPower;
    public bool debugPath;

    [SerializeField] private List<EnemySheep> sheeps;
    [SerializeField] private float aps, explosionRadius;

    PlacedBuilding placedBuilding;

    public enum attackMode { closest, First, Last, Strongest }
    [SerializeField] private attackMode currentAttackMode;


    [SerializeField] private float innerScanRadius, outScanRadius;


    private void Awake()
    {
        placedBuilding = GetComponent<PlacedBuilding>();
        sheeps = new List<EnemySheep>();
    }
    private void Start() { StartCoroutine(Scan()); }


    protected virtual IEnumerator Scan()
    {
        WaitForSeconds waitForNextScan = new WaitForSeconds(0.5f), nextAttack = new WaitForSeconds(1 / aps);

        while (true)
        {
            GetSurroundSheeps();
            if (sheeps.Count == 0) { yield return waitForNextScan; }
            else {
                switch (currentAttackMode) {
                    case attackMode.closest: attackClosest(); break;  case attackMode.First: attackFirst(); break;
                    case attackMode.Last: attackLast(); break; case attackMode.Strongest: attackStrongest(); break; }

                shoot();

                yield return nextAttack;
            }
            
        }
    }

    [SerializeField] private LayerMask sheepLayer;

    public void GetSurroundSheeps()
    {
        List<EnemySheep> removeSheeps = new List<EnemySheep>();
        sheeps.Clear();

        var outSurroundingObjects = Physics.OverlapSphere(pivotPoint.position, outScanRadius, sheepLayer);
        foreach (var surroundingObject in outSurroundingObjects)
        {
            EnemySheep enemySheep = surroundingObject.GetComponent<EnemySheep>();
            if (enemySheep != null) { sheeps.Add(enemySheep); }
        }

        if(sheeps.Count == 0) { return; }

        Debug.Log("BOm dia");

        var innerSurroundingObjects = Physics.OverlapSphere(pivotPoint.position, innerScanRadius, sheepLayer);
        foreach (var surroundingObject in innerSurroundingObjects)
        {
            EnemySheep enemySheep = surroundingObject.GetComponent<EnemySheep>();
            if (enemySheep != null) { removeSheeps.Add(enemySheep); }
        }

        foreach(EnemySheep es in removeSheeps) sheeps.Remove(es);

    }
    public void attackClosest(){

    }
    public void attackFirst(){
        while (sheeps.Count > 0)
        {
            if (sheeps[0] != null) { targetPos = sheeps[0].transform.position; targetSheep = sheeps[0]; break; }
            else sheeps.RemoveAt(0);
        }
    }
    public void attackLast()
    {
        while (sheeps.Count > 0)
        {
            if (sheeps[sheeps.Count - 1] != null) { targetPos = sheeps[sheeps.Count - 1].transform.position; break; }
            else sheeps.RemoveAt(sheeps.Count - 1);
        }
    }
    public void attackStrongest()
    {

    }



    public void Update()
    {

        if (targetPos == null) return;
        pivotPoint.transform.LookAt(new Vector3(targetPos.x,pivotPoint.position.y, targetPos.z));
        if (debugPath) { DrawPath(); }
    }


    //EnemySheep es = other.GetComponent<EnemySheep>();
    // if (es != null) { sheeps.Add(es); }

 
    private void shoot()
    {
        targetPos = targetSheep.getFuturePoint(5,3);

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