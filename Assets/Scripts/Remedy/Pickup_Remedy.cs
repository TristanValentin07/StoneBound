using UnityEngine;

public class RemedyPickup : MonoBehaviour
{
    public Item itemToPickup;         // Le remède à ramasser
    public GameObject pressEUI;       // L’UI “Appuyez sur E”
    public InventoryUI inventoryUI;   // UI de l’inventaire

    private bool playerInRange = false;
    private Inventory inventory = null;
    private int remedyOwned = 0;

    void Start()
    {
        // Cache le prompt au lancement
        if (pressEUI != null)
            pressEUI.SetActive(false);

        // Récupère dynamiquement l’InventoryUI si non assigné
        if (inventoryUI == null)
            inventoryUI = FindAnyObjectByType<InventoryUI>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (inventory == null)
            {
                Debug.LogWarning("❌ Impossible de ramasser le remède : pas d’inventaire trouvé.");
                return;
            }

            bool added = inventory.AddItem(itemToPickup);
            if (added)
            {
                remedyOwned++;
                Debug.Log($"✔ Remède ramassé ! ({remedyOwned} possédés)");

                // Mise à jour de l’UI
                if (inventoryUI != null)
                    inventoryUI.UpdateUI();

                // Masque le prompt et détruit l’objet
                if (pressEUI != null)
                    pressEUI.SetActive(false);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("❌ Inventaire plein, impossible de ramasser le remède.");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Récupère le composant Inventory du joueur
            inventory = other.GetComponent<Inventory>();
            if (inventory == null)
                Debug.LogError("❌ Le joueur n’a pas de script Inventory !");

            if (pressEUI != null)
                pressEUI.SetActive(true);
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (pressEUI != null)
                pressEUI.SetActive(false);
            playerInRange = false;
            inventory = null;
        }
    }
}
