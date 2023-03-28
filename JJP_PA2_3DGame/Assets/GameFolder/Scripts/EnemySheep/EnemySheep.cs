using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemySheep;

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

    protected SetTargetSheep setTargetSheep;

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


    public enum state { Idle, Chase, Atack, ChaseAtack }
 

    public void changeCurrentState(state state)
    {
        switch (state)
        {
            case state.Idle: StopAllCoroutines(); StartCoroutine(Idle()); break;
            case state.Chase: StopAllCoroutines(); StartCoroutine(Idle()); break;
            case state.Atack: StopAllCoroutines(); StartCoroutine(Idle()); break;
            case state.ChaseAtack: StopAllCoroutines(); StartCoroutine(Idle()); break;
            default: StopAllCoroutines(); StartCoroutine(Idle()); break;
        }
    }



    public void Awake()
    {
        setStats();
        navMeshAgent = GetComponent<NavMeshAgent>();
        setTargetSheep = GetComponent<SetTargetSheep>();
    }

    public void Start()
    {
        //addDificultyLevel();
        navMeshAgent.speed = speed;

    }


    public void Update() {
        if (healthPoints <= 0) OnDeath();
    }

   


    protected virtual IEnumerator Idle() {
        yield return null;
    }
    protected virtual IEnumerator Chase() {
        yield return null;
    }

    protected virtual IEnumerator Atack() {
        yield return null;
    }

    protected virtual IEnumerator ChaseAtack() {
        yield return null;
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
