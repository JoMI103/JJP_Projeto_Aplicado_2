using System;
using System.Drawing;
using UnityEngine;



public class PumpkinTrebuchet : Tower
{
    const string attackAnimationName = "ShootPumpkin";


    [Space(10)] [Header("PumpkinTrebuchetAtributes")] [Space(10)]
    [SerializeField] private Transform pumpkimPrefab;
    [SerializeField] private Transform shotPoint, pivotPoint;
    [SerializeField] private float gravity, maxHeight, addHeightCurve;
    public bool debugPath;
    [SerializeField] private float explosionRadius;

    protected override void Start()
    {
        base.Start();
        animator.speed = aps;

            
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
           if(clip.name == attackAnimationName) { nextAttack = new WaitForSeconds( clip.length); break; }
        }

    }


    public void Update()
    {
        if (targetSheep == null) return;

        Vector3 toPosition = (new Vector3(targetPos.x, pivotPoint.position.y, targetPos.z) - pivotPoint.position).normalized;
        float angle = SignedAngleBetween( Vector3.forward,  toPosition, Vector3.up);
        pivotPoint.rotation = Quaternion.Euler(0, angle, 0);
        pivotPoint.localRotation = Quaternion.Euler(0, pivotPoint.localRotation.eulerAngles.y, 0);

        if (debugPath) { DrawPath(); }
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
        LaunchData launchData = calculateLaunchData();
        if (!checkPath(launchData)) return;
        Transform pumpkimProjectile = Instantiate(pumpkimPrefab, shotPoint.position, shotPoint.rotation);
        pumpkimProjectile.GetComponent<Rigidbody>().velocity = launchData.initialVelocity;
        pumpkimProjectile.GetComponent<PumpkinImpact>().SetExplosionStats(placedBuilding.buildingTypeSO.damage);
    }

    protected override void shootAnimation()
    {
        if (targetSheep == null) return;
        targetPos = targetSheep.getFuturePoint(5, calculateTime(targetSheep.transform.position) + 0.4f);
        animator.Play(attackAnimationName);
    }


    #region launchPathData


    float calculateTime(Vector3 pos)
    {
        float displacementY = pos.y - shotPoint.position.y;
        float highestPoint = addHeightCurve;
        if (displacementY > 0) highestPoint = displacementY + addHeightCurve;

        return MathF.Sqrt(-2 * highestPoint / gravity) + MathF.Sqrt(2 * (displacementY - highestPoint) / gravity);
    }

    LaunchData calculateLaunchData()
    {
        float displacementY = targetPos.y - shotPoint.position.y;
        float highestPoint = addHeightCurve;
        if (displacementY > 0) highestPoint = displacementY + addHeightCurve; 
   
        
        Vector3 displacementXZ = new Vector3(targetPos.x - shotPoint.position.x, 0, targetPos.z - shotPoint.position.z);
        float time = MathF.Sqrt(-2 * highestPoint / gravity) + MathF.Sqrt(2 * (displacementY - highestPoint) / gravity); 
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * highestPoint);
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

            Debug.DrawLine(previousDrawPoint, drawPoint, UnityEngine.Color.blue);

            if (Physics.Raycast(previousDrawPoint, direction, Vector3.Distance(previousDrawPoint, drawPoint), projectileIgnore)) return false;

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

        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireSphere(pivotPoint.position, innerScanRadius);
        Gizmos.color = UnityEngine.Color.green;
        Gizmos.DrawWireSphere(pivotPoint.position, outScanRadius);
    }

    #endregion
}