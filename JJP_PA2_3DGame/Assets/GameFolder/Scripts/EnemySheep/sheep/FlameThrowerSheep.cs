using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerSheep : EnemySheep
{
    [Space(10)]
    [Header("FlameThrower Sheep Atributes")]
    [Space(10)]

    public bool nothing;
    [SerializeField] private ParticleSystem flameParticles;

  

    protected override void MoveAnim()
    {
        animator.Play("Walk");
    }

    protected override void whenTargetDestroy()
    {
        flameParticles.Stop(false);
        targetedBuilding = null;
    }
    protected override void AttackAndAtackAnim()
    {
        flameParticles.gameObject.SetActive(true);
        flameParticles.Play(true);
    
      
    }
    
    
    [SerializeField] Transform startFirePos; [SerializeField]  float fireRadius; [SerializeField]  LayerMask targetMask;
    
    Tree targetTree;
     
#region FollowPath
    protected override  IEnumerator FollowPath() {
        
        if(!navMeshAgent.enabled) navMeshAgent.enabled = true;     
        MoveAnim(); navMeshAgent.speed = sheepSpeed;
        yield return null;
        
        currentTargetPos = ObjectivePosition.position;
        setTargetSheep.setStaticTarget(currentTargetPos); 
        //navMeshAgent.velocity = Vector3.zero;
        
        float timeScanTree = 0;
        while (true) {
            
            yield return new WaitForSeconds(FollowPathCD);
            timeScanTree += FollowPathCD;
            checkFrontSheepSpeed();
            
            if(targetedBuilding == null) checkFrontPath(SheepSO.AttackRange);
            
            if(targetedBuilding != null){
                currentState = state.AtackConstruction; 
                yield return StartCoroutine(AtackConstruction());  
                navMeshAgent.speed = sheepSpeed;
            } 
            
            
            
            if(timeScanTree > 5){
                if(ScanForTrees()){ //if it finds a construction, it assigns the event and changes the state
               
                    
                    yield return StartCoroutine(followPathToTree()); 
                    MoveAnim(); navMeshAgent.speed = sheepSpeed;
                    yield return null;       
                    currentTargetPos = ObjectivePosition.position;
                    setTargetSheep.setStaticTarget(currentTargetPos); 
  
                }
                timeScanTree = 0;
            }

        }
    }
    
#endregion

    [SerializeField] private LayerMask treeMask; [SerializeField] private float scanRadius;
    
    private bool ScanForTrees(){
        
        if( Random.Range(0,3) != 1) return false; //3 = 50% of searching for a building, 4 = 33.3% and so on
        
        //gets the objects in a sphere area and adds them to a list when they have a placebuilding component
        var surroundingObjects = Physics.OverlapSphere(transform.position,scanRadius,treeMask);
        List<Tree> trees = new List<Tree>();

        foreach(var surroundingObject in surroundingObjects) {           
            Tree building = surroundingObject.transform.parent.GetComponent<Tree>();
            if (building != null) { trees.Add(building);  }
        }
        
        //if list has element gets a random element
        if(trees.Count != 0){
            targetTree = trees[Random.Range(0,trees.Count)]; return true;
        } return false; 
    }
    
    
    private IEnumerator followPathToTree(){
        if(!navMeshAgent.enabled) navMeshAgent.enabled = true;     
        MoveAnim(); navMeshAgent.speed = sheepSpeed;
        yield return null;
        
        currentTargetPos = targetTree.transform.position;
        setTargetSheep.setStaticTarget(currentTargetPos, transform.position); 
         Debug.LogError(currentTargetPos +"1");
        
        while (!(targetTree == null)) {
            
     
            
            if(Vector3.Distance(transform.position,targetTree.transform.position)< SheepSO.AttackRange){
               
                yield return StartCoroutine(AtackTree());
            }
      
            yield return FollowPathCD;
        }
        yield return null;
        
    }

    private void fire(){
        var surroundingObjects = Physics.OverlapSphere(transform.position,fireRadius, targetMask);


        foreach(var surroundingObject in surroundingObjects) {
            PlacedBuilding building = surroundingObject.GetComponent<PlacedBuilding>();
            if (building != null) { 
                float dotproduct = Vector3.Dot(transform.forward,( building.CenterPosition.position-transform.position).normalized);
                if(dotproduct > 0.8f)  // area do cone
                building.takeDamge(sheepAttackDmg);}
            Tree tree = surroundingObject.transform.parent.GetComponent<Tree>();
          
            if(tree != null){
             
                //float dotproduct = Vector3.Dot(transform.forward,( building.CenterPosition.position-transform.position).normalized);
                //if(dotproduct < 0.8f)  // area do cone
                tree.giveDmg(sheepAttackDmg, true);
            }
        }     
    }
    
    protected  IEnumerator AtackTree(){
        if(!navMeshAgent.enabled) navMeshAgent.enabled = true;     
        navMeshAgent.speed = 0; navMeshAgent.velocity = Vector3.zero;
        transform.forward = (targetTree.transform.position - this.transform.position).normalized;
        WaitForSeconds wait = new WaitForSeconds(sheepAttackSpeed);
        AttackAndAtackAnim(); 
        
        yield return wait;
        while (!(targetTree == null)){
            Debug.LogError(targetTree);
            fire();
            yield return wait;
        }
    }
    
    protected override IEnumerator AtackConstruction(){
        if(!navMeshAgent.enabled) navMeshAgent.enabled = true;     
        navMeshAgent.speed = 0;
        navMeshAgent.velocity = Vector3.zero;
        transform.forward = (targetedBuilding.CenterPosition.position- this.transform.position).normalized;
        WaitForSeconds wait = new WaitForSeconds(sheepAttackSpeed);
        AttackAndAtackAnim(); 
        
        yield return wait;
        while (targetedBuilding != null){
            fire();
          
    
            yield return wait;
        }
    }
    
    
}

