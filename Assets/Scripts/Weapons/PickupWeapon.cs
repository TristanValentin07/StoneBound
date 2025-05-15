using System;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickup : MonoBehaviour
{
    public Item itemToPickup;
    public GameObject pressEUI; 
    public InventoryUI inventoryUI;
    
    private bool playerInRange = false;
    private Inventory inventory; // ➔ Reference fixée une fois pour toutes

    void Start()
    {
        pressEUI.SetActive(false);

        // Fixer l'inventaire une seule fois au début
        inventory = FindAnyObjectByType<Inventory>();
        if (inventory == null)
            Debug.LogError("❌ Aucun Inventory trouvé dans la scène !");
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (inventory != null && inventory.AddItem(itemToPickup))
            {
                Debug.Log("✔ Arme ramassée !");
                
                if (inventoryUI != null)
                    inventoryUI.UpdateUI();

                pressEUI.SetActive(false);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("❌ Pickup échoué : Inventaire plein ou introuvable");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pressEUI.SetActive(true);
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pressEUI.SetActive(false);
            playerInRange = false;
        }
    }
}