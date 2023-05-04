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
     *  FollowPath: Segue um caminho (calculado pelo sistema de navmesh) até um determinado objetivo
     *              Tem um sensor que verifica se tem uma parede no caminho
     *              Adicionar: sensor de Player
     *              
     *  AtackConstruction: Ataca a construções detatadas pelo sensor de cima
     *  
     *  AtackPlayer: Ataca o jogador
     *               Adicionar: Perseguir o jogador 
     *               
     */

    public enum state { Idle, FollowPath, AtackConstruction, AtackPlayer }

    //sheep's current state so the script can stop it without using StopAllCoroutines
    private IEnumerator currentState;
  
    public void changeCurrentState(state state) {
        switch (state) {
            case state.Idle: if(currentState != null) StopCoroutine(currentState); currentState = Idle(); StartCoroutine(currentState); break;
            case state.FollowPath: if (currentState != null) StopCoroutine(currentState); currentState = FollowPath(); StartCoroutine(currentState); break;
            case state.AtackConstruction: if (currentState != null) StopCoroutine(currentState); currentState = AtackConstruction(); StartCoroutine(currentState); break;
            case state.AtackPlayer: if (currentState != null) StopCoroutine(currentState); currentState = AtackPlayer(); StartCoroutine(currentState); break;
            default: if (currentState != null) StopCoroutine(currentState); currentState = Idle(); StartCoroutine(currentState); break;
        }
    }
    #endregion
    
    #region Stats
    //sets the sheep's stats with the scriptableObject
    private void setStats()
    {
        baseHealth = m_EnemySheepTypeSO.baseHealth;
        healthPoints = baseHealth;
        attackDmg = m_EnemySheepTypeSO.baseDmg;
        speed = m_EnemySheepTypeSO.baseSpeed;
        AttackRange = m_EnemySheepTypeSO.AttackRange;
        AttackSpeed = m_EnemySheepTypeSO.AttackSpeed;
        slowModifier = 1; 
        weaknessModifier = 1f;
    }

    protected int baseHealth; 
    protected int attackDmg; 
    protected float speed, AttackRange, AttackSpeed;    //SheepBaseStats
    [HideInInspector] public float slowModifier; // 1 normal Velocity
    protected float weaknessModifier; //Bufs Defufs
    protected int healthPoints;

    #endregion

    [Space(10)][Header("Enemy Sheep Atributes")][Space(10)]

    [SerializeField] EnemySheepTypeSO m_EnemySheepTypeSO;
    [SerializeField] protected Animator animator;
    [SerializeField] private UIHealthBar healthBar;

    protected SetTargetSheep setTargetSheep;
    protected NavMeshAgent navMeshAgent;
    protected Rigidbody sheepRigidBody;
    protected Transform playerPosition, ObjectivePosition;

    public void setPlayerAndObjective(Transform player, Transform finalObjective) {
        playerPosition = player; 
        ObjectivePosition = finalObjective;
        setTargetSheep.setStaticTarget(ObjectivePosition);
    }

    public void Awake()
    {
        setStats();
        navMeshAgent = GetComponent<NavMeshAgent>();
        setTargetSheep = GetComponent<SetTargetSheep>();
        sheepRigidBody = GetComponent<Rigidbody>();
    }

    public void Start()
    {
        //addDificultyLevel();
        navMeshAgent.speed = speed;
        changeCurrentState(state.FollowPath);
    }

    public void Update() {
//#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.I)) changeCurrentState(state.Idle);
        if (Input.GetKeyDown(KeyCode.O)) changeCurrentState(state.FollowPath);
        if (Input.GetKeyDown(KeyCode.Alpha0)) deathWithNoEffect();
        //#endif
        if (healthPoints <= 0) OnDeath();
    }

    #region Fix Orientation

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

    //a ovelha segue o caminho até o objetivo
    protected virtual IEnumerator FollowPath() {

        if(!navMeshAgent.enabled) navMeshAgent.enabled = true;
        setTargetSheep.setStaticTarget(ObjectivePosition); 
        yield return null;

        while (true)
        {
            MoveAnim();

            if(navMeshAgent.enabled)
            {
                navMeshAgent.SamplePathPosition(-1, AttackRange, out NavMeshHit navHit);
                if (navHit.mask == 16) 
                checkPathObstacles();
            }

            yield return new WaitForSeconds(0.2f);
        }

    }

    protected virtual IEnumerator AtackConstruction() {

        
        WaitForSeconds wait = new WaitForSeconds(AttackSpeed);

        yield return wait;

        while (placedBuilding != null)
        {
            Attack(); 
            yield return wait;
        }
    }

    protected virtual void MoveAnim()
    {

    }
    protected virtual void Attack()
    {
        placedBuilding.takeDamge(attackDmg);
    }

    protected virtual IEnumerator AtackPlayer() {
        yield return null;
    }

    #endregion

    #region checkObstacles

    protected PlacedBuilding placedBuilding;
    public LayerMask destructibleLayer;

    protected virtual void checkPathObstacles()
    {
        Vector3[] corners = new Vector3[2];
     
        int length = navMeshAgent.path.GetCornersNonAlloc(corners);

        if (navMeshAgent.hasPath) Debug.Log("simtem");

        if (length > 1)
        {  
            if (Physics.Raycast(corners[0], (corners[1] - corners[0]).normalized, out RaycastHit hit,
                AttackRange, destructibleLayer))
            {       
                if (hit.collider.TryGetComponent<PlacedBuilding>(out PlacedBuilding destructible))
                {
                   

                    if(placedBuilding == destructible)
                    {
                        placedBuilding.onDestroyEvent += whenTargetDestroy;
                        //LastPath = navMeshAgent.path;
                        setTargetSheep.setStaticTarget(placedBuilding.transform);
                        changeCurrentState(state.AtackConstruction);
                    }
                    placedBuilding = destructible;
                }
            }

        }
    }

    protected virtual void whenTargetDestroy()
    {
        changeCurrentState(state.FollowPath);
    }

    #endregion

    #region someCalculations
    //gets the future point for slower projectiles
    public Vector3 getFuturePoint(int precision ,float time)
    {
        if (navMeshAgent.path.corners.Length < precision) precision = navMeshAgent.path.corners.Length;
        Vector3[] corners = new Vector3[precision];
        navMeshAgent.path.GetCornersNonAlloc(corners);
        float walkDistance = time * (speed * slowModifier);

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
        navMeshAgent.speed = speed * slowPower;
        yield return new WaitForSeconds(effectTime);
        navMeshAgent.speed = speed;
        slowModifier = 1;
        slowEffectIsRunning = false;
    }

    public void startknockBackEffect(Vector3 s, float d) { StartCoroutine(knockBackEffect(s, d)); }

    [SerializeField] private LayerMask floorLayerMask;

    public virtual IEnumerator knockBackEffect(Vector3 direction, float force)
    {
        StopCoroutine(currentState);
        navMeshAgent.enabled = false;
        sheepRigidBody.isKinematic= false;
        sheepRigidBody.AddForce(( direction + new Vector3(0,0.2f,0))  * force , ForceMode.Impulse);
        yield return new WaitForSeconds(5);     

        deathWithNoEffect();
       
    }

    #endregion

    public bool rig;
    [SerializeField] private LayerMask floor;


    public void receiveDmg(int dmg)
    {
        healthPoints -= (int)(dmg * weaknessModifier);
        healthBar.SetHealthBarPercentage((float)healthPoints / baseHealth);
    }

    protected virtual void OnDeath()
    {
        if (placedBuilding != null) placedBuilding.onDestroyEvent -= whenTargetDestroy;
        Destroy(this.gameObject);
    }

    protected void deathWithNoEffect()
    {
        if (placedBuilding != null) placedBuilding.onDestroyEvent -= whenTargetDestroy;
        Destroy(this.gameObject);
    }
}
