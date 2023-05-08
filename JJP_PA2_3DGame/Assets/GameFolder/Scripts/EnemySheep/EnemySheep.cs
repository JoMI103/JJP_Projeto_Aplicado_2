using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySheep : MonoBehaviour
{
    #region States

    /*
     * 
     *  Idle: A ovelha fica parada 
     *  
     *  FollowPath: Segue um caminho (calculado pelo sistema de navmesh) at� um determinado objetivo
     *              Tem um sensor que verifica se tem uma parede no caminho
     *              Adicionar: sensor de Player
     *              
     *  AtackConstruction: Ataca a constru��es detatadas pelo sensor de cima
     *  
     *  AtackPlayer: Ataca o jogador
     *               Adicionar: Perseguir o jogador 
     *               
     */

    public enum state { Idle, FollowPath, AtackConstruction, AtackPlayer }

    //sheep's current state so the script can stop it without using StopAllCoroutines
    private IEnumerator currentState;
  
    [Header("NavmeshPiorityValues")]
    [SerializeField] private int followPathPValue;
    [SerializeField] private int AtackConstPValue;

  
    public void changeCurrentState(state state) {
        switch (state) {
            case state.Idle: 
                if(currentState != null) StopCoroutine(currentState); 
                currentState = Idle(); StartCoroutine(currentState); 
                navMeshAgent.avoidancePriority = followPathPValue;
              
            break;
           
            case state.FollowPath: 
                if (currentState != null) StopCoroutine(currentState); 
                currentState = FollowPath(); StartCoroutine(currentState); 
                navMeshAgent.avoidancePriority = followPathPValue;
            
            break;
           
            case state.AtackConstruction: 
                if (currentState != null) StopCoroutine(currentState); 
                currentState = AtackConstruction(); StartCoroutine(currentState); 
                navMeshAgent.avoidancePriority = AtackConstPValue;
            break;
           
            case state.AtackPlayer: 
                if (currentState != null) StopCoroutine(currentState);
                currentState = AtackPlayer(); StartCoroutine(currentState); 
                navMeshAgent.avoidancePriority = followPathPValue;
           
            break;
            
            default: 
                if (currentState != null) StopCoroutine(currentState); 
                currentState = Idle(); StartCoroutine(currentState); 
                navMeshAgent.avoidancePriority = followPathPValue;
              
            break;
        }
    }
    #endregion
    
    #region Stats
    //sets the sheep's stats with the scriptableObject
    private void setStats()
    {
        sheepWeigth = SheepSO.weigth;
        sheepBaseHealth = SheepSO.baseHealth;
        sheepHealthPoints = sheepBaseHealth;
        sheepAttackDmg = SheepSO.baseDmg;
        sheepSpeed = SheepSO.baseSpeed;
        sheepAttackSpeed = SheepSO.AttackSpeed;
        slowModifier = 1; 
        sheepWeaknessModifier = 1f;
    }

    protected int sheepBaseHealth,sheepAttackDmg,sheepHealthPoints; 
    protected float sheepSpeed, sheepAttackSpeed,sheepWeaknessModifier, sheepWeigth;    //SheepBaseStats
    [HideInInspector] public float slowModifier; // 1 normal Velocity

    #endregion

    [Space(10)][Header("Enemy Sheep Atributes")][Space(10)]
    [SerializeField] EnemySheepTypeSO SheepSO;
    [SerializeField] protected Animator animator;
    [SerializeField] private UIHealthBar healthBar;

    protected SetTargetSheep setTargetSheep;
    protected NavMeshAgent navMeshAgent;
    protected Rigidbody sheepRigidBody;
    protected Transform playerPosition, ObjectivePosition;

    public void setPlayerAndObjective(Transform player, Transform finalObjective) {
        playerPosition = player; 
        ObjectivePosition = finalObjective;
        if(navMeshAgent.enabled) setTargetSheep.setStaticTarget(ObjectivePosition);
    }

    public void Awake()
    {
        setStats();
        navMeshAgent = GetComponent<NavMeshAgent>();
        setTargetSheep = GetComponent<SetTargetSheep>();
        sheepRigidBody = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        //addDificultyLevel();
        navMeshAgent.speed = sheepSpeed;
        changeCurrentState(state.FollowPath);
    }

    public void Update() {
        //#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.I)) changeCurrentState(state.Idle);
        if (Input.GetKeyDown(KeyCode.O)) changeCurrentState(state.FollowPath);
        if (Input.GetKeyDown(KeyCode.Alpha0)) deathWithNoEffect();
        //#endif
        if (sheepHealthPoints <= 0) OnDeath();
    }

    #region Fix Orientation

    [Header("Sets the sheep normal to the floor normal")]
    [SerializeField] protected Transform model;
    [SerializeField] protected bool fixOrientation;
    [SerializeField] protected LayerMask floorToFixOrientation;

    //atualiza a normal do modelo com a normal do grid 
    private void LateUpdate()
    {
        if(!fixOrientation) { return; }
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0,0.1f,0), Vector3.down, out hit, floorToFixOrientation))
            model.rotation = Quaternion.Lerp(model.rotation, Quaternion.FromToRotation(model.up, hit.normal) * model.rotation, Time.deltaTime * 5);
    }

    #endregion

    #region sheepStates
    



    //nao faz nada
    protected virtual IEnumerator Idle() {
        navMeshAgent.enabled= false;

        while (true)
        {
            yield return new WaitForSeconds(1f);
        }   
    }

    //a ovelha segue o caminho at� o objetivo
    protected virtual IEnumerator FollowPath() {
        
        if(!navMeshAgent.enabled) navMeshAgent.enabled = true;     
        navMeshAgent.speed = sheepSpeed;
        
        setTargetSheep.setStaticTarget(ObjectivePosition); 
        yield return null;

        MoveAnim();
        while (true) {
            checkFrontSheepSpeed();
            if(navMeshAgent.enabled) checkPathObstacles();
            yield return new WaitForSeconds(0.05f);
         
        }

    }

    protected virtual IEnumerator AtackConstruction() {

        if(!navMeshAgent.enabled) navMeshAgent.enabled = true;     
        navMeshAgent.speed = 0;
        navMeshAgent.velocity = Vector3.zero;
        WaitForSeconds wait = new WaitForSeconds(sheepAttackSpeed); yield return wait;

        while (targetedBuilding != null){
            AttackAndAtackAnim(); 
            /*if(checkNewPath()){
                 targetedBuilding.onDestroyEvent -= whenTargetDestroy;      
                whenTargetDestroy();
            }
    */
            yield return wait;
        }
    }
    protected virtual void MoveAnim() { }
    protected virtual void AttackAndAtackAnim() { targetedBuilding.takeDamge(sheepAttackDmg); }
    protected virtual IEnumerator AtackPlayer() {
        yield return null;
    }

    #endregion

    #region checkObstacles

    [Header("Layers that the sheep can destroy")]
    [SerializeField] protected LayerMask destructibleLayer;
    protected PlacedBuilding targetedBuilding;
    [SerializeField] private float AttackRange;
    private Vector3 hitpos; //only debug
    
    protected virtual void checkPathObstacles()
    {
        navMeshAgent.SamplePathPosition(-1, AttackRange, out NavMeshHit navHit);
        hitpos = navHit.position;
        
        if (navHit.mask == 16) {
             if (Physics.Raycast(navHit.position,Vector3.up, out RaycastHit hit, 1, destructibleLayer))
            {       
                if (hit.collider.TryGetComponent<PlacedBuilding>(out PlacedBuilding destructible))
                {   
                    if(targetedBuilding == destructible)
                    {
                        targetedBuilding.onDestroyEvent += whenTargetDestroy;  
                        changeCurrentState(state.AtackConstruction);
                    }
                    targetedBuilding = destructible;
                }
            }
            
        }
    }
    
    [SerializeField] private LayerMask enemySheeps;
    [SerializeField] private Transform ScanFrontSheepStartPoint;
    [SerializeField] private float distanceScan;
    
    protected void checkFrontSheepSpeed(){
         Physics.Raycast(ScanFrontSheepStartPoint.position,transform.forward ,out RaycastHit hit ,distanceScan,enemySheeps);
        if(hit.collider == null) {navMeshAgent.speed = sheepSpeed; return;}         
        navMeshAgent.speed = sheepSpeed * (Vector3.Distance(this.transform.position, hit.collider.transform.position) / distanceScan);
    }
    
    
    private Vector3 navhit2;
    
    private bool checkNewPath(){
    
 
        navMeshAgent.SamplePathPosition(-1, AttackRange , out NavMeshHit navHit);
        navhit2 = navHit.position;
     
        if(navHit.mask == 16) return false;
   
        return true;
    }

    protected virtual void whenTargetDestroy()
    {
        changeCurrentState(state.FollowPath);
    }

    #endregion

    #region someCalculations
    //gets the future point for slower projectiles
    public virtual Vector3 getFuturePoint(int precision ,float time)
    {
        if (navMeshAgent.path.corners.Length < precision) precision = navMeshAgent.path.corners.Length;
        Vector3[] corners = new Vector3[precision];
        navMeshAgent.path.GetCornersNonAlloc(corners);
        float walkDistance = time * (sheepSpeed * slowModifier);

        for(int i = 1; i< precision; i++)
        {
            float distance = Vector3.Distance(corners[i-1],corners[i]);
            if(distance < walkDistance) walkDistance -= distance; else
            {
                return corners[i - 1] + (corners[i] - corners[i - 1]).normalized * walkDistance;
            }
        }

        return corners[corners.Length - 1]; 

    }
    #endregion

    #region Effects

    [HideInInspector] public bool slowEffectIsRunning = false;
    public IEnumerator currentSlowEffect;

    public void startCurrentSlowEffect() { StartCoroutine(currentSlowEffect); }

    public virtual IEnumerator slowEffect(float effectTime, float slowPower)
    {
        slowEffectIsRunning = true; 
        slowModifier = slowPower;
        navMeshAgent.speed = sheepSpeed * slowPower;
        yield return new WaitForSeconds(effectTime);
        navMeshAgent.speed = sheepSpeed;
        slowModifier = 1;
        slowEffectIsRunning = false;
    }

    public void startknockBackEffect(Vector3 s, float d) { StartCoroutine(knockBackEffect(s, d)); }
 


    bool knockBacking;
    public virtual IEnumerator knockBackEffect(Vector3 direction, float force)
    {
        StopCoroutine(currentState);
        navMeshAgent.enabled = false;
        sheepRigidBody.isKinematic= false;  
        sheepRigidBody.AddForce(( direction + new Vector3(0,0.2f,0))  * force * (1 / sheepWeigth) , ForceMode.Impulse);
        yield return new WaitForSeconds(1);
        knockBacking =true;
        yield return new WaitForSeconds(20); //morre em 20 segundos se nao encotrar a navmesh   

        deathWithNoEffect();
       
    }
    
    [SerializeField] private LayerMask navmeshFloor;
    protected virtual void OnCollisionEnter(Collision other) {
        Debug.Log("fewfwe");
        if(!knockBacking) return;
        if((navmeshFloor.value & (1 << other.gameObject.layer)) > 0){
            StopAllCoroutines();
            navMeshAgent.enabled = true;
             sheepRigidBody.isKinematic= true;
             knockBacking = false;
            changeCurrentState(state.FollowPath);
        }
    }
    

    #endregion

    #region DmgDeath

    public void receiveDmg(int dmg)
    {
        sheepHealthPoints -= (int)(dmg * sheepWeaknessModifier);
        healthBar.SetHealthBarPercentage((float)sheepHealthPoints / sheepBaseHealth);
    }

    protected virtual void OnDeath()
    {
        if (targetedBuilding != null) targetedBuilding.onDestroyEvent -= whenTargetDestroy;
        Destroy(this.gameObject);
    }

    protected void deathWithNoEffect()
    {
        if (targetedBuilding != null) targetedBuilding.onDestroyEvent -= whenTargetDestroy;
        Destroy(this.gameObject);
    }

    #endregion

    #region gizmos
#if UNITY_EDITOR

    protected virtual void OnDrawGizmosSelected() {
        Gizmos.DrawLine(hitpos,hitpos+Vector3.up);
        Gizmos.color = Color.red;
        if(navhit2 != Vector3.zero){
            
        Gizmos.DrawSphere(transform.position + transform.forward * AttackRange, 0.2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(navhit2 , 0.2f);
        
        }
        Gizmos.DrawLine(ScanFrontSheepStartPoint.position, ScanFrontSheepStartPoint.position + transform.forward * distanceScan);
    }

#endif
    #endregion
}
