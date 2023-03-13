using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySheep : MonoBehaviour
{
    //SheepBaseStats
    int baseHealth;
    int attackDmg;
    float speed;

    //Bufs Defufs
    float slow;
    float weaknessMult;

    int healthPoints;

    private void setStats()
    {
        baseHealth = m_EnemySheepTypeSO.baseHealth;
        healthPoints = baseHealth;
        attackDmg = m_EnemySheepTypeSO.baseDmg;
        speed = m_EnemySheepTypeSO.baseSpeed;

        slow = 0; weaknessMult = 1f;
    }

    [SerializeField] EnemySheepTypeSO m_EnemySheepTypeSO;

    private NavMeshAgent navMeshAgent;
    [SerializeField] private UIHealthBar healthBar;

    public void Awake()
    {
        setStats();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void Start()
    {
        //addDificultyLevel();
        navMeshAgent.speed = speed;
    }


    public void Update()
    {
        if (healthPoints <= 0) OnDeath();
    }


 
    public void receiveDmg(int dmg)
    {
        healthPoints -= (int)(dmg * weaknessMult);
        healthBar.SetHealthBarPercentage((float)healthPoints/ baseHealth);
    }


    protected virtual void OnDeath()
    {
        Destroy(this.gameObject);
    }
  
}
