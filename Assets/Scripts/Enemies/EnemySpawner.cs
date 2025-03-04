using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnRate = 10f;

    private bool spawning = true;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (spawning)
        {
            yield return new WaitForSeconds(spawnRate);

            if (enemyPrefab != null && spawnPoints.Length > 0)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
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