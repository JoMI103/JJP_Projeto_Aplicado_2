using System.Collections;
using UnityEngine;
using UnityEngine.AI;


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
        AtackRange = m_EnemySheepTypeSO.AtackRange;
        slow = 0; weaknessMult = 1f;
    }

    //SheepBaseStats
    int baseHealth; int attackDmg; float speed, AtackRange;

    //Bufs Defufs
    float slow; float weaknessMult;

    int healthPoints;

    [SerializeField] EnemySheepTypeSO m_EnemySheepTypeSO;


    [SerializeField] private UIHealthBar healthBar;

    protected SetTargetSheep setTargetSheep;
    protected NavMeshAgent navMeshAgent;
    protected Rigidbody rigidbody;

    protected Transform playerPosition, ObjectivePosition;
    public void setPlayerAndObjective(Transform player, Transform finalObjective) {
        playerPosition = player; setTargetSheep.setTarget(finalObjective);
        ObjectivePosition = finalObjective;
    }


    public void Awake()
    {
        setStats();
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

    protected virtual IEnumerator Idle() {
        navMeshAgent.enabled= false;

        while (true)
        {

            yield return new WaitForSeconds(1f);
        }   
    }
    protected virtual IEnumerator FollowPath() {
        navMeshAgent.enabled = true;
        while (true)
        {
            checkPathObstacles();

            yield return new WaitForSeconds(0.2f);
        }
    }
    protected virtual IEnumerator AtackConstruction() {

        navMeshAgent.enabled = false;
        WaitForSeconds wait = new WaitForSeconds(1f);

        while (placedBuilding != null)
        {
            placedBuilding.takeDamge(attackDmg);
            yield return wait;
        }
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


    protected PlacedBuilding placedBuilding;
    public LayerMask destructibleLayer;

    protected virtual void checkPathObstacles()
    {
        Vector3[] corners = new Vector3[2];
     
        int length = navMeshAgent.path.GetCornersNonAlloc(corners);

        if (length > 1)
        {  
            if (Physics.Raycast(corners[0], (corners[1] - corners[0]).normalized, out RaycastHit hit,
                AtackRange, destructibleLayer))
            {       
                if (hit.collider.TryGetComponent<PlacedBuilding>(out PlacedBuilding destructible))
                {
                    placedBuilding = destructible;
                    placedBuilding.onDestroyEvent += whenTargetDestroy;
                    //LastPath = navMeshAgent.path;
                    changeCurrentState(state.AtackConstruction);
                }
            }

        }
    }

    private void whenTargetDestroy()
    {
        changeCurrentState(state.FollowPath);
    }

}
