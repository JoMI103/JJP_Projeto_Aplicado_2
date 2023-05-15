using System.Collections;

using UnityEngine;

public class FlameThrowerSheep : EnemySheep
{
    [Space(10)]
    [Header("FlameThrower Sheep Atributes")]
    [Space(10)]

    public bool nothing;
    [SerializeField] private GameObject flameParticles;

    protected override void MoveAnim()
    {
        animator.Play("Walk");
    }

    protected override void whenTargetDestroy()
    {
        flameParticles.SetActive(false);
        //changeCurrentState(state.FollowPath);
    }
    protected override void AttackAndAtackAnim()
    {
        //StartFire Animation pedro
        flameParticles.SetActive(true);
      
    }
    
    
    [SerializeField] Transform startFirePos; [SerializeField]  float fireRadius; [SerializeField]  LayerMask targetMask;
    
    
    private void fire(){
        var surroundingObjects = Physics.OverlapSphere(transform.position,fireRadius, targetMask);


        foreach(var surroundingObject in surroundingObjects) {
            PlacedBuilding building = surroundingObject.GetComponent<PlacedBuilding>();
            if (building != null) { 
                float dotproduct = Vector3.Dot(transform.forward,( building.transform.position-transform.position).normalized);
                if(dotproduct > 0.8f)  // area do cone
                building.takeDamge(sheepAttackDmg);}
        }     
    }
    
    protected override IEnumerator AtackConstruction(){
        if(!navMeshAgent.enabled) navMeshAgent.enabled = true;     
        navMeshAgent.speed = 0;
        navMeshAgent.velocity = Vector3.zero;
        WaitForSeconds wait = new WaitForSeconds(sheepAttackSpeed); yield return wait;
        AttackAndAtackAnim(); 
        while (targetedBuilding != null){
            fire();
          
    
            yield return wait;
        }
    }
    
    #region  gizmos
    #if UNITY_EDITOR
    private bool DebugGizmos;
    
    private void OnDrawGizmos() {
        if(DebugGizmos)
        Gizmos.DrawWireSphere(startFirePos.position,fireRadius);
    }
    
    #endif
    #endregion
}
