using System;
using UnityEngine;
using UnityEngine.UI;

public class RemedyPickup : MonoBehaviour
{
    public Item itemToPickup;
    public GameObject pressEUI; // UI press E
    public InventoryUI inventoryUI;

    private bool playerInRange = false;
    private int remedyOwned;

    void Start()
    {
        pressEUI.SetActive(false);
    }
    
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Inventory inventory = FindAnyObjectByType<Inventory>();
            if (inventory.AddItem(itemToPickup))
            {
                Debug.Log("✔ Remede ramassée !");
                remedyOwned++;
                Debug.Log(remedyOwned + "/3 remedes possédés");
                inventoryUI.UpdateUI();
                pressEUI.SetActive(false);
                Destroy(gameObject); // supprimer le remède ramassée de la scène
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