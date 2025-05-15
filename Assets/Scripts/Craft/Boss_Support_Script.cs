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
    public float delayBeforeMenu = 3f; // Temps avant de revenir au menu

    private bool primeDeposited = false;
    private bool playerInRange = false;
    private WeaponManager playerWeaponManager;
    private GameObject placedPrime;

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
        if (victoryScreen != null)
        {
            victoryScreen.gameObject.SetActive(true);
        }
        StartCoroutine(BackToMenu());
    }

    System.Collections.IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(delayBeforeMenu);
        SceneManager.LoadScene(menuSceneName);
    }
}
