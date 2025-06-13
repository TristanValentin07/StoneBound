using UnityEngine;
using UnityEngine.UI;

public class WeaponPickup : MonoBehaviour
{
    public Item itemToPickup;
    public GameObject pressEUI; 
    public InventoryUI inventoryUI;

    private bool playerInRange = false;
    private Inventory inventory = null;

    void Start()
    {
        if (pressEUI != null)
            pressEUI.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (inventory == null)
            {
                Debug.LogWarning("❌ Impossible de ramasser : pas d'inventaire trouvé.");
                return;
            }

            bool added = inventory.AddItem(itemToPickup);
            if (added)
            {
                Debug.Log("✔ Arme ramassée et ajoutée à l'inventaire.");

                if (inventoryUI != null)
                    inventoryUI.UpdateUI();

                if (pressEUI != null)
                    pressEUI.SetActive(false);

                Destroy(gameObject); // Supprime l'objet ramassé
            }
            else
            {
                Debug.LogWarning("❌ Inventaire plein, impossible de ramasser.");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventory = other.GetComponent<Inventory>();

            if (inventory == null)
                Debug.LogError("❌ Le joueur est entré mais n'a pas de script Inventory !");

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