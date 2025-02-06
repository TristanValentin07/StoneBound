using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private GameObject playerInstance;

    void Start()
    {
        SpawnPlayer();
        Debug.Log("Joueur Spawned: " + playerInstance.name);
    }

    void SpawnPlayer()
    {
        Transform spawnPoint = GameObject.Find("SpawnPoint")?.transform;
        if (spawnPoint == null)
        {
            Debug.LogError("SpawnPoint introuvable dans la map !");
            return;
        }

        if (playerInstance == null)
        {
            playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            playerInstance.SetActive(true);

        }
    }
}
