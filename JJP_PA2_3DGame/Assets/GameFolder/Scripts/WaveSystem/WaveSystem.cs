using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static EnemyWaveSO;

public class WaveSystem : MonoBehaviour
{
    [SerializeField] private Transform waveTarget,playerTarget;
    [SerializeField] private List<Transform> spawnPoints;
    private enemyWaveData currentWaveData;
    
   [HideInInspector] public int currentWave, currentPartWave;
    
    
    public int startWave;
    [SerializeField]private LevelWavesSO LevelWaves;
 

    [SerializeField] private Transform waveSheepsFolder;

    private void Start()
    {
        currentWaveData = new enemyWaveData(1);
        currentWave = startWave;
        if(nextWave()) StartCoroutine(SpawnNextWave());
    }

    [SerializeField] private waveUI waveui;
    
    [HideInInspector] public int nSheeps;


    private void Update() {
        nSheeps =  waveSheepsFolder.childCount;
    }

    IEnumerator SpawnNextWave()
    {
        int currentWaveCooldown = LevelWaves.waves[currentWave].cooldown;
        waveui.StartCount(currentWaveCooldown);
        yield return new WaitForSeconds(LevelWaves.waves[currentWave].cooldown);
        
        waveEnd = false;
        currentPartWave = -1;
        StartCoroutine(Wave());
       
        do {
            yield return new WaitForSeconds(1f);
        } while (!waveEnd);
        
        Debug.LogError("fdspa");
        spawnWaveEnded = false;
        if (nextWave()) StartCoroutine(SpawnNextWave());
    }
    
    
    [HideInInspector]public bool waveEnd = true ,spawnWaveEnded;
  
    
    IEnumerator Wave()
    {
       
        do
        {
            if(!spawnWaveEnded){
                spawnWaveEnded = getPartWaveEnemies();
                if(!spawnWaveEnded){
                    
                yield return new WaitForSeconds(currentWaveData.partWaveCooldown);
                
                do{
                SpawnPartWave();
                yield return new WaitForSeconds(1.5f);
            
                }while(currentWaveData.getWaveQuantity() > 0 );
                }
            }

            
            yield return new WaitForSeconds(1);
            
        } while (!spawnWaveEnded || waveSheepsFolder.childCount > 0);
        waveEnd = true;
    }
    
    


    private void SpawnPartWave()
    {
        foreach(Transform sp in spawnPoints)
        {
            if (currentWaveData.getWaveQuantity() == 0) break;
            int n = Random.Range(0, currentWaveData.typeEnemies.Count);
            WaveEnemy _waveEnemy = currentWaveData.typeEnemies[n];


            Transform sheep = Instantiate(_waveEnemy.enemySheep.prefab, sp);
            sheep.SetParent(waveSheepsFolder);
            sheep.GetComponent<EnemySheep>().setPlayerAndObjective(playerTarget,waveTarget);

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
            return true;
        }
        return false;
    }
    
    private bool getPartWaveEnemies(){
        currentPartWave++;
      
        
        if(currentPartWave >= LevelWaves.waves[currentWave].waveSO.wavePart.Count) return true;
        
        if(currentPartWave < -1){currentPartWave = 0; Debug.LogWarning("Alguma coisa de errado nao esta certa");}
        
        currentWaveData.typeEnemies = new List<WaveEnemy>();
        currentWaveData.partWaveCooldown = LevelWaves.waves[currentWave].waveSO.wavePart[currentPartWave].cooldown;
        
        foreach(WaveEnemy w in LevelWaves.waves[currentWave].waveSO.wavePart[currentPartWave].typeEnemies)
        {
                if(w.Quantity > 0) currentWaveData.typeEnemies.Add(w);
        }
            
        return false;
    }
    
 /*
  currentWaveData.typeEnemies = new List<WaveEnemy>();
            foreach(WaveEnemy w in LevelWaves.waves[currentWave].waveSO?.typeEnemies)
            {
                if(w.Quantity > 0) currentWaveData.typeEnemies.Add(w);
            }
 */

}

public class enemyWaveData
{
    public List<WaveEnemy> typeEnemies;

    public int partWaveCooldown;

    public int getWaveQuantity()
    {
        int quant = 0;
        foreach (WaveEnemy enemy in typeEnemies) { quant += enemy.Quantity; }
        return quant;
    }

    public enemyWaveData(int ok)
    {
        
    }
}
