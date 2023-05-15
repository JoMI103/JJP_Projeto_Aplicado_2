using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaloonSheep : EnemySheep
{
    [Space(10)] [Header("Baloon Sheep Atributes")] [Space(10)]
    
    [SerializeField] Transform aeroBombaPrefab;   [SerializeField] Transform aeroBombaSpawn;
    private Vector3 currentTargetPos;  private bool stopMoving; private float distanceToObjective;

#region SheepStates
    
    protected override void MoveAnim() {  stopMoving = false; animator.Play("Walk"); }

    private const float checkUpdateYposTime = 0.5f;

    protected override IEnumerator FollowPath()
    {
        if(navMeshAgent.enabled) navMeshAgent.enabled = false; 
        currentTargetPos = ObjectivePosition.position;   
        yPercent = 1;
        MoveAnim();
        
        yield return null;

        float time = 0;
        while (true) {
            if(time > 10){
                if(scanBuildings()){ //if it finds a construction, it assigns the event and changes the state
                    targetedBuilding.onDestroyEvent += whenTargetDestroy;  
                    //changeCurrentState(state.AtackConstruction);
                }
                time = 0;
            }
            //checks the distance to the ground to get the target Y Pos
            time += checkUpdateYposTime; fixYPos();
            yield return new WaitForSeconds(checkUpdateYposTime);
        }
    }
    
    
    [SerializeField] private LayerMask buildingMask; [SerializeField] private float scanRadius;
    
    private bool scanBuildings(){
        
        if( Random.Range(0,3) != 1) return false; //3 = 50% of searching for a building, 4 = 33.3% and so on
        
        //gets the objects in a sphere area and adds them to a list when they have a placebuilding component
        var surroundingObjects = Physics.OverlapSphere(transform.position +transform.forward * (scanRadius),scanRadius,buildingMask);
        List<PlacedBuilding> buildings = new List<PlacedBuilding>();

        foreach(var surroundingObject in surroundingObjects) {           
            PlacedBuilding building = surroundingObject.GetComponent<PlacedBuilding>();
            if (building != null) { buildings.Add(building); }
        }
        
        //if list has element gets a random element
        if(buildings.Count != 0){
            targetedBuilding = buildings[Random.Range(0,buildings.Count)]; return true;
        } return false; 
    }
    
    protected override IEnumerator AtackConstruction(){
         yPercent = 0.75f;
        
        if(navMeshAgent.enabled) navMeshAgent.enabled = false;     
        currentTargetPos = targetedBuilding.GetComponent<SphereCollider>().center + targetedBuilding.transform.position;
        
        MoveAnim();
        yield return null;
        
        //when timeStack is greater than attack Speed a bomb is dropped to affect all constructions around
        float timeAtack = 0;
        while(true){
            timeAtack += Time.deltaTime;
            if(distanceToObjective < 1f){
                if(timeAtack > sheepAttackSpeed){
                    AttackAndAtackAnim();
                    timeAtack = 0;
                }
            }
            else MoveAnim();
            
            yield return null;
        }
    }

    protected override void AttackAndAtackAnim() {
        //Animator atack
        stopMoving = true;
        sheepRigidBody.velocity = Vector3.zero;
        Instantiate(aeroBombaPrefab,aeroBombaSpawn.position, Quaternion.identity).GetComponent<aeroBomba>().SetExplosionStats(sheepAttackDmg,buildingMask);
    }
    
    //protected override void whenTargetDestroy() { changeCurrentState(state.FollowPath); }
    
#endregion
    
    
#region SheepMovement

    [SerializeField] private float aMax; 
    
    private void FixedUpdate() {
        //current position with the targetPosition.y and gets the distance
        Vector3 _currentPos = transform.position; 
        _currentPos.y = currentTargetPos.y;      
        distanceToObjective = Vector3.Distance(_currentPos,currentTargetPos);
        
        if(stopMoving) return;

        if(distanceToObjective > 0.5f) {
            Vector3 vIdeal = currentTargetPos - _currentPos;
            vIdeal = vIdeal.normalized * sheepSpeed;
            Vector3 acceleration = (vIdeal - sheepRigidBody.velocity).normalized * aMax;
            
            sheepRigidBody.AddForce(acceleration, ForceMode.Force);
            
            if(sheepRigidBody.velocity == Vector3.zero) transform.forward = Vector3.forward; else
            transform.forward = Vector3.Lerp(transform.forward,sheepRigidBody.velocity.normalized,Time.deltaTime);
        }
    }

    [SerializeField] private LayerMask ground;
    [SerializeField] private float yOffSet;  private float yPos, yPercent = 1;
    
    
    private void fixYPos(){
        Physics.Raycast(transform.position + new Vector3(0,100,0) + sheepRigidBody.velocity.normalized * 5,Vector3.down,out RaycastHit hit,200, ground);
        
        if(hit.collider != null) { 
             yPos = hit.point.y+ yOffSet * yPercent;
        }
    }
    
     private void LateUpdate() {
        Vector3 _currentPos = transform.position;
        transform.position = new Vector3(_currentPos.x, Mathf.Lerp(_currentPos.y,yPos,Time.deltaTime ), _currentPos.z);
    }
    
#endregion


    #region OtherMethods
        public override Vector3 getFuturePoint(int precision ,float time){
            return transform.position + sheepRigidBody.velocity  * time * Random.Range(0.7f,1f);
        }
        
    #endregion

    #region Gizmos
   
    #if UNITY_EDITOR
 
 
    #endif
 
    #endregion
}
