using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAllEnemies : MonoBehaviour
{

   void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach(Transform enemy in transform)
            {
                enemy.GetComponent<EnemyHealthDrops>().Die();               
            }
            Debug.Log("Kill all enemies");
        }
    }
}
