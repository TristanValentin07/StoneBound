using UnityEngine;
using System.Collections.Generic;

public class RemedySpawner : MonoBehaviour
{
    [Header("Remède Prefabs")]
    public GameObject[] remedyPrefabs; // Les 3 prefabs (Green Flask, Red Flask, Syringe)

    [Header("Points de Spawn Possibles")]
    public Transform[] spawnPoints; //Les points de spawn

    private List<Transform> availableSpawnPoints = new List<Transform>();

    void Start()
    {
        SpawnRemedies();
    }

    void SpawnRemedies()
    {
        if (remedyPrefabs.Length > spawnPoints.Length)
        {
            Debug.LogError("Pas assez de points de spawn pour le nombre d'objets !");
            return;
        }

        // Remplir la liste des points disponibles
        availableSpawnPoints.AddRange(spawnPoints);

        foreach (var remedyPrefab in remedyPrefabs)
        {
            if (availableSpawnPoints.Count == 0)
            {
                Debug.LogWarning("Plus de points disponibles !");
                break;
            }

            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[randomIndex];

            Instantiate(remedyPrefab, spawnPoint.position, spawnPoint.rotation);

            availableSpawnPoints.RemoveAt(randomIndex); // On enlève le point utilisé
        }
    }
}