using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private GameObject playerInstance;
    private Player_Data playerData;

    private Health_Manager healthManager;
    private Stamina_Manager staminaManager;

    void Start()
    {
        healthManager = FindAnyObjectByType<Health_Manager>();
        staminaManager = FindAnyObjectByType<Stamina_Manager>();

        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        Transform spawnPoint = GameObject.Find("SpawnPoint")?.transform;
        if (spawnPoint == null)
        {
            Debug.LogError("SpawnPoint introuvable !");
            return;
        }

        if (playerInstance == null)
        {
            playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            playerInstance.SetActive(true);
            playerData = new Player_Data();

            if (playerData != null)
            {
                healthManager.SetHealthToPlayer(playerData);
                staminaManager.SetStaminaToPlayer(playerData);
            }
        }
    }
}