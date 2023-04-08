using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyWaveSO;

public class SimpleWaveSpawnerDebug : MonoBehaviour
{
    [SerializeField] private Transform waveTarget, playerTarget;
    [SerializeField] private EnemySheepTypeSO enemySheepTypeSO;
    
    [SerializeField, Range(1,100)] int numberSheep;
    [SerializeField] float coolDown;

    private void Start()
    {
        StartCoroutine(SpawnWave());

        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StopAllCoroutines();
            StartCoroutine(SpawnWave());
        }
    }

    IEnumerator SpawnWave()
    {
        for(int i  = 0; i < numberSheep; i++)
        {
            yield return new WaitForSeconds(coolDown);
            Transform sheep = Instantiate(enemySheepTypeSO.prefab, this.transform);
            sheep.GetComponent<EnemySheep>().setPlayerAndObjective(playerTarget, waveTarget);
        }
    }
}
