using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContaminatedSheep : EnemySheep
{
    [Space(10)] [Header("Contaminated Sheep Atributes")] [Space(10)]

    public bool nothing;

    protected override void MoveAnim()
    {
        animator.Play("Walk");
    }
  
    
    
    
    #region FollowPath
    protected override IEnumerator FollowPath() {
        
        if(!navMeshAgent.enabled) navMeshAgent.enabled = true;     
        MoveAnim(); navMeshAgent.speed = sheepSpeed;
        yield return null;
        
        currentTargetPos = ObjectivePosition.position;
        setTargetSheep.setStaticTarget(currentTargetPos); 
        //navMeshAgent.velocity = Vector3.zero;
        
        while (true) {
            yield return new WaitForSeconds(FollowPathCD);
            
            checkFrontSheepSpeed();
            
            if(targetedBuilding == null) checkFrontPath(SheepSO.AttackRange);
            
            if(targetedBuilding != null){
                currentState = state.AtackConstruction; 
                yield return StartCoroutine(AtackConstruction());  
                navMeshAgent.speed = sheepSpeed;
            } 
            
            if(Vector3.Distance(transform.position, playerPosition.position) < SheepSO.AttackRange){
                 yield return StartCoroutine(atackPlayer());  
                 navMeshAgent.speed = sheepSpeed;
            }
        }
    }
    
    
    
#endregion
    
    [SerializeField] Transform projectilePrefab;
    
    protected override void AttackAndAtackAnim()
    {
        
        Instantiate(projectilePrefab, ScanFrontSheepStartPoint.position + transform.forward, Quaternion.identity).GetComponent<MossProjectile>
        ().shootBuilding(targetedBuilding.GetComponent<SphereCollider>().center + targetedBuilding.transform.position,targetedBuilding,sheepAttackDmg,20);
    }
    
    private IEnumerator atackPlayer(){
        navMeshAgent.speed = 0;
        Instantiate(projectilePrefab, ScanFrontSheepStartPoint.position + transform.forward, Quaternion.identity).GetComponent<MossProjectile>
        ().shootPlayer(playerPosition.GetComponent<PlayerStats>(),sheepAttackDmg, 20);
        yield return new WaitForSeconds( sheepAttackSpeed);
    }
}
