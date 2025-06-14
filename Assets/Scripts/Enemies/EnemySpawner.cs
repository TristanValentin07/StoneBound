using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnRate = 10f;
    public int maxEnemies = 3;

    private bool spawning = true;
    private Queue<GameObject> enemyPool = new Queue<GameObject>();

    void Start()
    {
        var diff = GameSettings.difficulty;

        spawnRate  = GameSettings.SpawnRate(diff);
        maxEnemies = GameSettings.MaxEnemies(diff);

        for (int i = 0; i < maxEnemies; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
        }

        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (spawning)
        {
            yield return new WaitForSeconds(spawnRate);

            // On ne spawn un ennemi que s'il y en a de disponibles dans la pool
            if (enemyPool.Count > 0)
            {
                // Choix aléatoire d'un point de spawn
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                // Récupération et activation de l'ennemi
                GameObject enemy = enemyPool.Dequeue();
                enemy.transform.position = spawnPoint.position;
                enemy.transform.rotation = spawnPoint.rotation;
                enemy.SetActive(true);
            }
        }
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);
    }

    public void ToggleSpawning(bool isActive)
    {
        spawning = isActive;
        if (isActive)
        {
            StartCoroutine(SpawnEnemies());
        }
    }
}