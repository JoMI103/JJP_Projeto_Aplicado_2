using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySheep : MonoBehaviour
{
    #region States

    public enum state { Idle, FollowPath, AtackConstruction, AtackPlayer }
    private state currentState;
  
   // [Header("NavmeshPiorityValues")]
   // [SerializeField] private int followPathPValue; [SerializeField] private int AtackConstPValue;

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

    [Space(10)] [Header("Enemy Sheep Atributes")] [Space(10)]
    
    [SerializeField] EnemySheepTypeSO SheepSO;   [SerializeField] protected Animator animator; [SerializeField] private UIHealthBar healthBar;
    protected SetTargetSheep setTargetSheep;  protected NavMeshAgent navMeshAgent;  protected Rigidbody sheepRigidBody;
    protected Transform playerPosition, ObjectivePosition;
    
    protected Vector3 currentTargetPos;

    public void Awake() {
        setStats();
        navMeshAgent = GetComponent<NavMeshAgent>();
        setTargetSheep = GetComponent<SetTargetSheep>();
        sheepRigidBody = GetComponent<Rigidbody>();
        navMeshAgent.stoppingDistance = SheepSO.AttackRange;
        navMeshAgent.speed = sheepSpeed;
    }
    
    public void setPlayerAndObjective(Transform player, Transform finalObjective) {
        playerPosition = player; 
        ObjectivePosition = finalObjective;
    }

    protected virtual void Start() {
        //currentState = state.FollowPath;
       //StartCoroutine(FollowPath());
       Invoke("test",0.3f);
    }

    protected virtual void Update() {
        
       
        
        
        //#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.I)) currentState = state.Idle;
        if (Input.GetKeyDown(KeyCode.O))  currentState = state.FollowPath;
       // if (Input.GetKeyDown(KeyCode.Alpha0)) deathWithNoEffect();
        //#endif
        if (sheepHealthPoints <= 0) OnDeath();
    }
    
    private void test(){
         currentTargetPos = ObjectivePosition.position;

         setTargetSheep.setStaticTarget(currentTargetPos); 

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

#region Idle
    protected virtual IEnumerator Idle() {
        navMeshAgent.enabled= false;
        while (currentState == state.Idle) {
            yield return new WaitForSeconds(1f);
        }   
    }
    
#endregion
    
#region FollowPath
    protected virtual IEnumerator FollowPath() {
        
        if(!navMeshAgent.enabled) navMeshAgent.enabled = true;     
        MoveAnim(); navMeshAgent.speed = sheepSpeed;
        yield return null;
        
        currentTargetPos = ObjectivePosition.position;
        GetNewPath(false); setTargetSheep.setStaticTarget(currentTargetPos); 
        //navMeshAgent.velocity = Vector3.zero;
        
        while (true) {
            
            switch (currentState)
            {
                case state.Idle: yield return StartCoroutine(Idle()); break;
                case state.AtackConstruction: {
                    yield return StartCoroutine(AtackConstruction());  
                    navMeshAgent.speed = sheepSpeed;
                    currentTargetPos = ObjectivePosition.position;
                    } break;
                default: break;
            }
  
            GetNewPath(false);
            setTargetSheep.setStaticTarget(currentTargetPos); 
            
            yield return new WaitForSeconds(0.3f);
       
  
            
            if(targetedBuilding != null){
      
                if(navMeshAgent.GetPathRemainingDistance()  < navMeshAgent.stoppingDistance){
                    
                    currentState = state.AtackConstruction;
                }        
            }
        }
    }
    
    
#endregion



#region AttackConstruction
    protected virtual IEnumerator AtackConstruction() {

        if(!navMeshAgent.enabled) navMeshAgent.enabled = true;     
        navMeshAgent.speed = 0; navMeshAgent.velocity = Vector3.zero;
        WaitForSeconds wait = new WaitForSeconds(sheepAttackSpeed); 

        while (currentState == state.AtackConstruction){
            yield return wait;
        
            if(targetedBuilding != null){
                AttackAndAtackAnim(); 
              
            }else{
                currentState = state.FollowPath;
            }
            
        }
        yield return null;
    }
    
#endregion
    
#region AtackPlayer
    protected virtual IEnumerator AtackPlayer() {
        yield return null;
    }
#endregion
    
    protected virtual void MoveAnim() { }
    protected virtual void AttackAndAtackAnim() { targetedBuilding.takeDamge(sheepAttackDmg); }
   
    protected virtual void whenTargetDestroy()
    {
        targetedBuilding = null;
    }

#region checkPathForObstacles
        
    [Header("Layers that the sheep can destroy")]
    [SerializeField] protected LayerMask buildingTypeFocus;
    protected PlacedBuilding targetedBuilding;
 
        
    private void GetNewPath(bool atacking){

        
        NavMeshPath pathAllAreas = new NavMeshPath();  int n = NavMesh.AllAreas;
        
        currentTargetPos = ObjectivePosition.position;
        NavMesh.CalculatePath(transform.position, currentTargetPos ,n, pathAllAreas);
    
        
        
        for (int i = 0; i < pathAllAreas.corners.Length - 1; i++)
             if(Physics.Linecast(pathAllAreas.corners[i], pathAllAreas.corners[i + 1],out RaycastHit hit ,buildingTypeFocus)){
                if (hit.collider.TryGetComponent<PlacedBuilding>(out PlacedBuilding currentTargetBuilding)) {  
                    currentTargetPos =  hit.point;
                      
                    
                    if(targetedBuilding == null){
                        targetedBuilding = currentTargetBuilding;
                        targetedBuilding.onDestroyEvent += whenTargetDestroy;  
                      
                    }else
                    if(atacking && targetedBuilding != currentTargetBuilding){                    
                        targetedBuilding.onDestroyEvent -= whenTargetDestroy;
                        targetedBuilding = currentTargetBuilding;
                        targetedBuilding.onDestroyEvent += whenTargetDestroy;  
                        //changeCurrentState(state.FollowPath);
                    }
                }            
                return;        
             }
    }
        
     
        
#endregion

#region checkFrontSheepToReduceVelocity
        [SerializeField] Transform ScanFrontSheepStartPoint; [SerializeField] float distanceScan;
        [SerializeField] LayerMask enemySheeps;
        
        protected void checkFrontSheepSpeed(){
             Physics.Raycast(ScanFrontSheepStartPoint.position,transform.forward ,out RaycastHit hit ,distanceScan,enemySheeps);
            if(hit.collider == null) {navMeshAgent.speed = sheepSpeed; return;}         
            navMeshAgent.speed = sheepSpeed * (Vector3.Distance(this.transform.position, hit.collider.transform.position) / distanceScan);
        }
    
    
#endregion
    
#region FuturePoint
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

        if(corners.Length >0)
            return corners[corners.Length - 1]; 
        return transform.position;
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
       // StopCoroutine(currentState);
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
           // currentTargetPos =  ObjectivePosition.position; changeCurrentState(state.FollowPath);
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
        addResources();
        Destroy(this.gameObject);
    }

    protected void deathWithNoEffect()
    {
        if (targetedBuilding != null) targetedBuilding.onDestroyEvent -= whenTargetDestroy;
        addResources();
        Destroy(this.gameObject);

    }

    public void death() { this.deathWithNoEffect(); }

    protected void addResources()
    {
        playerPosition.GetComponent<PlayerStats>().eletronicsQuantity += 2;
        playerPosition.GetComponent<PlayerStats>().metalQuantity += 20;

    }


    #endregion

#region gizmos
#if UNITY_EDITOR

 

#endif
#endregion
}
public static class ExtensionMethods
{
    public static float GetPathRemainingDistance(this NavMeshAgent navMeshAgent)
    {
        if (navMeshAgent.pathPending ||
            navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid ||
            navMeshAgent.path.corners.Length == 0)
            return -1f;

        float distance = 0.0f;
        for (int i = 0; i < navMeshAgent.path.corners.Length - 1; ++i)
        {
            distance += Vector3.Distance(navMeshAgent.path.corners[i], navMeshAgent.path.corners[i + 1]);
        }

        return distance;
    }
}
