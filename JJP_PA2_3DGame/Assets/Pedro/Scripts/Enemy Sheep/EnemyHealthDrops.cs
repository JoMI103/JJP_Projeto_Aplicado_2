using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthDrops : MonoBehaviour
{
    private int health;
    [Header("Define Health and Drops")] public EnemyType enemyType;
    [SerializeField] GameObject[] drops;

    public enum EnemyType
    {
        standard,
        contaminated,
        baloon,
        timeBomb,
    }


    // Start is called before the first frame update
    void Start()
    {
        switch (enemyType)
        {
            case EnemyType.standard:
                health = 50;
            break;
            case EnemyType.contaminated:
                health = 70;
            break;
            case EnemyType.baloon:
                health = 40;
            break;
            case EnemyType.timeBomb:
                health = 30;
            break;


        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) Die();
    }

    public void TakeDamage(int dmgT)
    {
        health -= dmgT;
    }

    public void Die()
    {
        for (int i = 0; i < drops.Length; i++) Instantiate(drops[i], transform.position + new Vector3(0,1,0), Quaternion.identity);
        Destroy(gameObject);
    }
}
