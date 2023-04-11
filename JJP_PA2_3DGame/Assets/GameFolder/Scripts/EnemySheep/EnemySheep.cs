using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySheep : MonoBehaviour
{
    public enum state { Idle, FollowPath, AtackConstruction, AtackPlayer }

    private IEnumerator currentState;
  
    public void changeCurrentState(state state)
    {
        switch (state)
        {
            case state.Idle: if(currentState != null) StopCoroutine(currentState); currentState = Idle(); StartCoroutine(currentState); break;
            case state.FollowPath: if (currentState != null) StopCoroutine(currentState); currentState = FollowPath(); StartCoroutine(currentState); break;
            case state.AtackConstruction: if (currentState != null) StopCoroutine(currentState); currentState = AtackConstruction(); StartCoroutine(currentState); break;
            case state.AtackPlayer: if (currentState != null) StopCoroutine(currentState); currentState = AtackPlayer(); StartCoroutine(currentState); break;
            default: if (currentState != null) StopCoroutine(currentState); currentState = Idle(); StartCoroutine(currentState); break;
        }
    }

    private void setStats()
    {
        baseHealth = m_EnemySheepTypeSO.baseHealth;
        healthPoints = baseHealth;
        attackDmg = m_EnemySheepTypeSO.baseDmg;
        speed = m_EnemySheepTypeSO.baseSpeed;
        AttackRange = m_EnemySheepTypeSO.AttackRange;
        AttackSpeed = m_EnemySheepTypeSO.AttackSpeed;
        slow = 0; weaknessMult = 1f;
    }

    protected int baseHealth; protected int attackDmg; protected float speed, AttackRange, AttackSpeed;    //SheepBaseStats
    public float slow; protected float weaknessMult; //Bufs Defufs

    protected int healthPoints;

    [SerializeField] EnemySheepTypeSO m_EnemySheepTypeSO;


    [SerializeField] private UIHealthBar healthBar;

    protected SetTargetSheep setTargetSheep;
    protected NavMeshAgent navMeshAgent;
    protected Rigidbody rigidbody;
    [SerializeField] protected Animator animator;

    protected Transform playerPosition, ObjectivePosition;
    public void setPlayerAndObjective(Transform player, Transform finalObjective) {
        playerPosition = player; 
        setTargetSheep.setTarget(finalObjective);
        ObjectivePosition = finalObjective;
    }

    public void Awake()
    {
        setStats();
        //animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        setTargetSheep = GetComponent<SetTargetSheep>();
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Start()
    {
        //addDificultyLevel();
        navMeshAgent.speed = speed;
        changeCurrentState(state.FollowPath);
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.I)) changeCurrentState(state.Idle);
        if (Input.GetKeyDown(KeyCode.O)) changeCurrentState(state.FollowPath);

        if (healthPoints <= 0) OnDeath();
    }

    [SerializeField] protected Transform model;
    [SerializeField] protected bool fixRotation;
    [SerializeField] protected LayerMask l;

    //atualiza a normal do modelo com a normal do grid 
    private void LateUpdate()
    {
        if(!fixRotation) { return; }
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0,0.1f,0), Vector3.down, out hit, l))
            model.rotation = Quaternion.Lerp(model.rotation, Quaternion.FromToRotation(model.up, hit.normal) * model.rotation, Time.deltaTime * 5);
    }

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
        setTargetSheep.setTarget(ObjectivePosition); 
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
                        setTargetSheep.setTarget(placedBuilding.transform);
                        changeCurrentState(state.AtackConstruction);
                    }
                    placedBuilding = destructible;
                }
            }

        }
    }

    private void whenTargetDestroy()
    {
        changeCurrentState(state.FollowPath);
    }

    #endregion

    public void receiveDmg(int dmg)
    {
        healthPoints -= (int)(dmg * weaknessMult);
        healthBar.SetHealthBarPercentage((float)healthPoints / baseHealth);
    }

    protected virtual void OnDeath()
    {
        if (placedBuilding != null) placedBuilding.onDestroyEvent -= whenTargetDestroy;
        Destroy(this.gameObject);
    }

    public Vector3 getFuturePoint(int precision ,float time)
    {
        if (navMeshAgent.path.corners.Length < precision) precision = navMeshAgent.path.corners.Length;
        Vector3[] corners = new Vector3[precision];
        navMeshAgent.path.GetCornersNonAlloc(corners);
        float walkDistance = time * (speed * slow);

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

    #region Effects

    public bool slowEffectIsRunning = false;
    public IEnumerator currentSlowEffect;

    public void startCurrentSlowEffect() { StartCoroutine(currentSlowEffect); }

    public virtual IEnumerator slowEffect(float effectTime, float slowPower)
    {
        slowEffectIsRunning = true; 
        slow = slowPower;
        navMeshAgent.speed = speed * slowPower;
        yield return new WaitForSeconds(effectTime);
        navMeshAgent.speed = speed;
        slow = 1;
        slowEffectIsRunning = false;
    }

    public void startknockBackEffect(Vector3 s, float d) { StartCoroutine(knockBackEffect(s, d)); }

    [SerializeField] private LayerMask floorLayerMask;

    public virtual IEnumerator knockBackEffect(Vector3 direction, float force)
    {
        StopCoroutine(currentState);
        navMeshAgent.enabled = false;
        rigidbody.isKinematic= false;
        rigidbody.AddForce(( direction + new Vector3(0,0.2f,0))  * force , ForceMode.Impulse);
        yield return new WaitForSeconds(5);     

        deathWithNoEffect();
       
    }

    #endregion

    public bool rig;
    [SerializeField] private LayerMask floor;



    protected void deathWithNoEffect()
    {
        if (placedBuilding != null) placedBuilding.onDestroyEvent -= whenTargetDestroy;
        Destroy(this.gameObject);
    }

}
