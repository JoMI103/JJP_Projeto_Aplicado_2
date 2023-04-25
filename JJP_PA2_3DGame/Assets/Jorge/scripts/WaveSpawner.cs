using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Transform target;
    public Transform enemyPrefab;
    public float spawnInterval = 2f;
    public int numberOfEnemies = 10;

    private int enemiesSpawned = 0;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (enemiesSpawned < numberOfEnemies)
        {
            Transform sheep = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            sheep.GetComponent<SetTargetSheep>().setStaticTarget(target);
            enemiesSpawned++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
        if (enemiesSpawned >= numberOfEnemies)
        {
            Debug.Log("Wave complete!");
        }
    }
}
