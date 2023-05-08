using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaloonSheep : EnemySheep
{
    [Space(10)]
    [Header("Baloon Sheep Atributes")]
    [Space(10)]

    public bool nothing;

    

    protected override void MoveAnim()
    {
        animator.Play("Walk");
    }

    private Vector3 currentTarget;

    protected override IEnumerator FollowPath()
    {
        if(navMeshAgent.enabled) navMeshAgent.enabled = false;     
        currentTarget = ObjectivePosition.position;
            
        yield return null;

        MoveAnim();
        float time = 0;
        while (true) {
            
            if(time > 10){
                if( scanBuildings()){
                    targetedBuilding.onDestroyEvent += whenTargetDestroy;  
                    changeCurrentState(state.AtackConstruction);
                }
                time = 0;
            }
            
            time += 0.5f;
           fixYPos();
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    protected override IEnumerator AtackConstruction(){
        if(navMeshAgent.enabled) navMeshAgent.enabled = false;     
        currentTarget = targetedBuilding.GetComponent<SphereCollider>().center + targetedBuilding.transform.position;
        yield return null;
        
        MoveAnim();
        
        float timeAtack = 0;
        while(true){
            
            timeAtack += Time.deltaTime;
            if(distanceObjective < 1f){
                if(timeAtack > AttackSpeed){
                    AttackAndAtackAnim();
                    timeAtack = 0;
                }
            }else{
                MoveAnim();
                stopMoving = false;
            }
            yield return null;
        }
    }
    
    [SerializeField] Transform aeroBombaPrefab;
    [SerializeField] Transform aeroBombaSpawn;
    
    protected override void AttackAndAtackAnim()
    {
        //Animator atack
       aeroBomba aeroBombaS =  Instantiate(aeroBombaPrefab,aeroBombaSpawn.position, Quaternion.identity).GetComponent<aeroBomba>();
       aeroBombaS.SetExplosionStats(attackDmg,buildingMask);
       stopMoving = true;
       sheepRigidBody.velocity = Vector3.zero;
    }
    
    protected override void whenTargetDestroy()
    {
        changeCurrentState(state.FollowPath);
    }
    
    [SerializeField] private LayerMask buildingMask;
    [SerializeField] private float scanRadius;
    
    private bool scanBuildings(){
        if( Random.Range(0,3) != 1) return false;
        var surroundingObjects = Physics.OverlapSphere(transform.position +transform.forward * (scanRadius),scanRadius,buildingMask);

        List<PlacedBuilding> buildings = new List<PlacedBuilding>();

        foreach(var surroundingObject in surroundingObjects)
        {
           
            PlacedBuilding building = surroundingObject.GetComponent<PlacedBuilding>();
            if (building != null) {buildings.Add(building); }
        }
        
        if(buildings.Count != 0){
            targetedBuilding = buildings[Random.Range(0,buildings.Count)];
            return true;
        }
        
        return false;
    }
    
    [SerializeField] private float aMax;
    private float distanceObjective;
    private bool stopMoving;
    
    private void FixedUpdate() {
        Vector3 p1 = transform.position;
        p1.y = currentTarget.y; 
        
        distanceObjective = Vector3.Distance(p1,currentTarget);
        
        if(stopMoving) return;

        if(distanceObjective > 0.1f){
            Vector3 vIdeal = currentTarget - p1;
            vIdeal = vIdeal.normalized * speed;
            Vector3 acceleration = (vIdeal - sheepRigidBody.velocity).normalized * aMax;
            sheepRigidBody.AddForce(acceleration, ForceMode.Force);
            if(sheepRigidBody.velocity == Vector3.zero) transform.forward = Vector3.forward; else
            transform.forward = sheepRigidBody.velocity.normalized;
        }
    }

    
    [SerializeField] private LayerMask ground;
    [SerializeField] private float yOffSet;
    
    private float yPos;
    
    private void fixYPos(){
        Physics.Raycast(transform.position + new Vector3(0,100,0) + sheepRigidBody.velocity.normalized * 5,Vector3.down,out RaycastHit hit,200, ground);
        
        if(hit.collider != null){
            
             yPos = hit.point.y+ yOffSet;
        }
    }
    
     private void LateUpdate() {

        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x,Mathf.Lerp(pos.y,yPos,Time.deltaTime ),pos.z);
    }


    public override Vector3 getFuturePoint(int precision ,float time){
        return transform.position + sheepRigidBody.velocity  * time * Random.Range(0.6f,1.1f);
    }

#if UNITY_EDITOR

protected override void OnDrawGizmosSelected() {
    base.OnDrawGizmosSelected();
    Gizmos.DrawWireSphere(transform.position +transform.forward * (scanRadius),scanRadius);
}

#endif

}
