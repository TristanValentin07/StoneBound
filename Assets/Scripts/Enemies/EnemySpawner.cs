using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnRate = 10f;
    public int maxEnemies = 3;

    // Indique si le spawner est actif
    private bool spawning = true;
    // Pool des ennemis disponibles
    private Queue<GameObject> enemyPool = new Queue<GameObject>();

    void Start()
    {
        // Pré-instanciation de la pool avec le nombre maximum d'ennemis
        for (int i = 0; i < maxEnemies; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
        }

        // Démarre la coroutine de spawn
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

    // Méthode à appeler par l'ennemi lorsqu'il meurt ou doit être réutilisé
    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);
    }

    // Permet d'activer/désactiver le spawn via un appel externe
    public void ToggleSpawning(bool isActive)
    {
        spawning = isActive;
        if (isActive)
        {
            StartCoroutine(SpawnEnemies());
        }
    }
}