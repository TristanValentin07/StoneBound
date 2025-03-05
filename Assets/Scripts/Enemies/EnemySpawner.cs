using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnRate = 10f;
    public int maxEnemies = 3;

    private bool spawning = true;
    private bool maxSpawned = false;
    private int counter = 0;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (spawning && maxSpawned == false)
        {
            yield return new WaitForSeconds(spawnRate);

            if (enemyPrefab != null && spawnPoints.Length > 0)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                counter++;
                if (counter >= maxEnemies)
                {
                    maxSpawned = true;
                }
            }
            else
            {
                Debug.LogWarning("❌ Aucun prefab d'ennemi ou spawn points non définis !");
            }
        }
    }

    // Pour activer/désactiver le spawn
    public void ToggleSpawning(bool isActive)
    {
        spawning = isActive;
        if (isActive)
        {
            StartCoroutine(SpawnEnemies());
        }
    }
}