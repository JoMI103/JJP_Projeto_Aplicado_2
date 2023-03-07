using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static EnemyWaveSO;

public class WaveSystem : MonoBehaviour
{
    [SerializeField] private Transform waveTarget;
    [SerializeField] private List<Transform> spawnPoints;
    private enemyWaveData currentWaveData;
    private int currentWave;
    public int startWave;
    [SerializeField]private LevelWavesSO LevelWaves;
    private List<Transform> currentWaveEnemies;
    

    private void Start()
    {
        currentWaveData = new enemyWaveData(1);
        currentWave = startWave;

        if(nextWave()) StartCoroutine(SpawnNextWave());
    }

    void Update()
    {
        
    }
    IEnumerator SpawnNextWave()
    {
        yield return new WaitForSeconds(LevelWaves.waves[currentWave].cooldown);

        currentWaveEnemies = new List<Transform>();
        do
        {
            SpawnPartWave();

            yield return new WaitForSeconds(2f);
        } while (currentWaveData.getWaveQuantity() > 0 && checkSheepAlive());


        if (nextWave())
        SpawnNextWave();
    }


    private void SpawnPartWave()
    {
        foreach(Transform sp in spawnPoints)
        {
            if (currentWaveData.getWaveQuantity() == 0) break;

            int n = Random.Range(0, currentWaveData.typeEnemies.Count);

            WaveEnemy _waveEnemy = currentWaveData.typeEnemies[n];

            Transform sheep = Instantiate(_waveEnemy.enemySheep.prefab, sp);
            sheep.GetComponent<SetTargetSheep>().setTarget(waveTarget);

            currentWaveEnemies.Add(sheep);
            _waveEnemy.Quantity--;
            if (_waveEnemy.Quantity == 0) { currentWaveData.typeEnemies.RemoveAt(n); } else
            { currentWaveData.typeEnemies[n] = _waveEnemy; }
        }
   
    }

    private bool nextWave()
    {
        currentWave++;
        if(currentWave < LevelWaves.waves.Count)
        {
            currentWaveData.typeEnemies = new List<WaveEnemy>();
            foreach(WaveEnemy w in LevelWaves.waves[currentWave].waveSO?.typeEnemies)
            {
                currentWaveData.typeEnemies.Add(w);
            }
           

            return true;
        }
        return false;
    }

    private bool checkSheepAlive()
    {
        foreach(Transform sp in spawnPoints)
        {
            if(sp.childCount > 0) { return true; }
        }
        return false;
    }

}

public class enemyWaveData
{
    public List<WaveEnemy> typeEnemies;

    public int getWaveQuantity()
    {
        int quant = 0;
        foreach (WaveEnemy enemy in typeEnemies) { quant += enemy.Quantity; }
        Debug.Log(quant);
        return quant;
    }

    public enemyWaveData(int ok)
    {
        
    }
}
