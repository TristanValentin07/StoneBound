using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonScript : MonoBehaviour
{
    public GameObject playerPrefab;
    private GameObject playerInstance;
    [SerializeField] private string sceneToLoad = "Map Tristan";

    public void StartGame()
    {
        Debug.Log("Chargement de la scène : " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
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
