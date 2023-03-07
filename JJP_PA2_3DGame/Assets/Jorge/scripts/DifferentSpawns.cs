using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifferentSpawns : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public float spawnDelay = 1f;
    public int enemiesPerWave = 1;
    public int waves = 1;
    public Transform[] spawnPoints;

    private int currentWave = 0;
    private int enemiesSpawned = 0;
    private bool isWaveInProgress = false;

    IEnumerator Start()
    {
        while (currentWave < waves)
        {
            yield return StartCoroutine(SpawnWave());
            currentWave++;
        }
    }

    IEnumerator SpawnWave()
    {
        isWaveInProgress = true;
        while (enemiesSpawned < enemiesPerWave)
        {
            int enemyIndex = Random.Range(0, enemyPrefabs.Count);
            GameObject enemyPrefab = enemyPrefabs[enemyIndex];

            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnIndex];

            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            enemiesSpawned++;

            yield return new WaitForSeconds(spawnDelay);
        }

        while (isWaveInProgress)
        {
            yield return null;
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                isWaveInProgress = false;
            }
        }

        enemiesSpawned = 0;
    }
}