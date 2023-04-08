using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;


public class EnemySheep : MonoBehaviour
{
    public enum state { Idle, FollowPath, AtackConstruction, AtackPlayer }

    public void changeCurrentState(state state)
    {
        switch (state)
        {
            case state.Idle: StopAllCoroutines(); StartCoroutine(Idle()); break;
            case state.FollowPath: StopAllCoroutines(); StartCoroutine(FollowPath()); break;
            case state.AtackConstruction: StopAllCoroutines(); StartCoroutine(AtackConstruction()); break;
            case state.AtackPlayer: StopAllCoroutines(); StartCoroutine(AtackPlayer()); break;
            default: StopAllCoroutines(); StartCoroutine(Idle()); break;
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

    //SheepBaseStats
    protected int baseHealth; protected int attackDmg; protected float speed, AttackRange, AttackSpeed;

    //Bufs Defufs
    protected float slow; protected float weaknessMult;

    protected int healthPoints;

    [SerializeField] EnemySheepTypeSO m_EnemySheepTypeSO;


    [SerializeField] private UIHealthBar healthBar;

    protected SetTargetSheep setTargetSheep;
    protected NavMeshAgent navMeshAgent;
    protected Rigidbody rigidbody;
    protected Animator animator;

    protected Transform playerPosition, ObjectivePosition;
    public void setPlayerAndObjective(Transform player, Transform finalObjective) {
        playerPosition = player; 
        setTargetSheep.setTarget(finalObjective);
        ObjectivePosition = finalObjective;
    }


    public void Awake()
    {
        setStats();
        animator = GetComponent<Animator>();
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
            navMeshAgent.SamplePathPosition(-1, AttackRange, out NavMeshHit navHit);
            if (navHit.mask == 16) 
            checkPathObstacles();

            yield return new WaitForSeconds(0.2f);
        }

    }
    
    
    protected virtual IEnumerator AtackConstruction() {

        WaitForSeconds wait = new WaitForSeconds(AttackSpeed);

        while (placedBuilding != null)
        {
            Attack(); 
            yield return wait;
        }
    }

    protected virtual void Attack()
    {
        placedBuilding.takeDamge(attackDmg);
    }

    protected virtual IEnumerator AtackPlayer() {
        yield return null;
    }

    public void receiveDmg(int dmg)
    { 
        healthPoints -= (int)(dmg * weaknessMult);
        healthBar.SetHealthBarPercentage((float)healthPoints/ baseHealth);
    }

    protected virtual void OnDeath()
    {
        if(placedBuilding != null) placedBuilding.onDestroyEvent -= whenTargetDestroy;
        Destroy(this.gameObject);
    }

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
}
