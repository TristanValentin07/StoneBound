using UnityEngine;
using UnityEngine.SceneManagement; // Pour recharger ou changer de scène
using UnityEngine.UI; // Pour afficher "YOU WIN"

public class VictorySupport : MonoBehaviour
{
    public string requiredItemName = "Boss Prime";
    public Transform primeSlot;
    public GameObject uiDepose;
    public GameObject uiActivate;
    public Image victoryScreen;
    public string menuSceneName = "Menu"; // Nom de la scène du menu principal

    private bool primeDeposited = false;
    private bool playerInRange = false;
    private WeaponManager playerWeaponManager;
    private GameObject placedPrime;
    
    private EnemyWaveSpawnerPool _nbrKill;
    public Text nbr_kill_text;
    public Text timePlayedText;
    private GameTimer _gameTimer;

    void Update()
    {
        if (!playerInRange || playerWeaponManager == null)
        {
            HideUI();
            return;
        }

        GameObject heldObject = playerWeaponManager.currentWeaponGO;

        if (!primeDeposited)
        {
            if (heldObject != null && heldObject.name.Contains(requiredItemName))
            {
                uiDepose?.SetActive(true);
                uiActivate?.SetActive(false);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    placedPrime = Instantiate(heldObject, primeSlot.position, primeSlot.rotation, transform);
                    placedPrime.transform.localScale = Vector3.one * 0.3f;

                    if (placedPrime.TryGetComponent<RemedyPickup>(out var pickup))
                    {
                        pickup.enabled = false;
                    }

                    primeDeposited = true;

                    playerWeaponManager.UnEquipWeapon();
                    Destroy(heldObject);

                    HideUI();
                }
            }
            else
            {
                HideUI();
            }
        }
        else
        {
            uiDepose?.SetActive(false);
            uiActivate?.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                TriggerVictory();
            }
        }
    }

    void HideUI()
    {
        uiDepose?.SetActive(false);
        uiActivate?.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerWeaponManager = other.GetComponentInChildren<WeaponManager>();
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerWeaponManager = null;
            playerInRange = false;
            HideUI();
        }
    }

    void TriggerVictory()
    {
        Debug.Log("🏆 VICTORY !");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (victoryScreen != null)
        {
            victoryScreen.gameObject.SetActive(true);
        }
        _nbrKill = FindAnyObjectByType<EnemyWaveSpawnerPool>();
        if (_nbrKill != null && nbr_kill_text != null)
        {
            nbr_kill_text.text = "Kills : " + _nbrKill.KillCount.ToString();
        }
        _gameTimer = FindAnyObjectByType<GameTimer>();
        if (_gameTimer != null && timePlayedText != null)
        {
            float timeRemaining = _gameTimer.GetTimeRemaining();
            float duration = GameSettings.TimerDuration(GameSettings.difficulty);
            float elapsedTime = duration - timeRemaining;

            Debug.Log($"⏱ Temps restant : {timeRemaining:F2} sec");
            Debug.Log($"⏳ Durée totale : {duration:F2} sec");
            Debug.Log($"🕓 Temps écoulé : {elapsedTime:F2} sec");

            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            timePlayedText.text = $"Time elapsed : {minutes:00}:{seconds:00}";
        }
        else
        {
            Debug.LogWarning("⚠️ GameTimer ou timePlayedText est null !");
        }

    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
