using UnityEngine;
using System.Collections.Generic;

public class CraftingSupport : MonoBehaviour
{
    [Header("Objets requis et slots de dépôt")]
    public List<string> requiredItemNames; // Stock des items requis pour le craft
    public Transform[] objectSlots; // La position des objets sur le support
    public GameObject passPrefab; // Le prefab de l'anneau de pouvoir
    public Transform passSpawnPoint; // La position du spawn de l'anneau

    [Header("UI")]
    public GameObject uiDepose; // UI pour déposer
    public GameObject uiCraft;  // UI pour crafter
    
    [Header("Dynamic Power Ring UI assignation")]
    public GameObject PressEUI;
    public InventoryUI inventoryUI;

    private Dictionary<string, GameObject> depositedItems = new Dictionary<string, GameObject>(); // Les items déjà posés sur le support
    private bool playerInRange = false; // Check pour savoir si le joueur est assez proche du support
    private WeaponManager playerWeaponManager; // La classe qui contient le code de gestion des armes / objets équipés

    void Update()
    {
        if (!playerInRange || playerWeaponManager == null)
        {
            HideAllUI();
            return;
        }

        GameObject heldObject = playerWeaponManager.currentWeaponGO;

        // Affichage contextuel
        if (depositedItems.Count == requiredItemNames.Count)
        {
            ShowCraftUI();
        }
        else if (heldObject != null)
        {
            string itemName = heldObject.name.Replace("(Clone)", "").Trim();
            if (requiredItemNames.Contains(itemName) && !depositedItems.ContainsKey(itemName))
            {
                ShowDeposeUI();
            }
            else
            {
                HideAllUI();
            }
        }
        else
        {
            HideAllUI();
        }

        // Interaction
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (depositedItems.Count == requiredItemNames.Count)
            {
                foreach (var obj in depositedItems.Values)
                    Destroy(obj);

                depositedItems.Clear();

                GameObject newPass = Instantiate(passPrefab, passSpawnPoint.position, passSpawnPoint.rotation);

                // Assignation dynamique des références pour qu'il soit ramassable
                RemedyPickup pickup = newPass.GetComponent<RemedyPickup>();
                if (pickup != null)
                {
                    pickup.pressEUI = PressEUI;
                    pickup.inventoryUI = inventoryUI;
                }

                HideAllUI();
                return;
            }


            if (heldObject != null)
            {
                Debug.Log($"🎒 Objet en main : {(heldObject != null ? heldObject.name : "Aucun")}");
                string itemName = heldObject.name.Replace("(Clone)", "").Trim();

                if (requiredItemNames.Contains(itemName) && !depositedItems.ContainsKey(itemName))
                {
                    int slotIndex = depositedItems.Count;
                    Transform targetSlot = objectSlots[slotIndex];

                    GameObject placedObj = Instantiate(heldObject, targetSlot.position, targetSlot.rotation, transform);
                    placedObj.transform.localScale = Vector3.one * 0.2f; // reglage de la taille des objets sur le support
                    depositedItems[itemName] = placedObj;

                    playerWeaponManager.UnEquipWeapon();
                    Destroy(heldObject);
                }
            }
        }
    }

    void ShowDeposeUI()
    {
        if (uiDepose != null) uiDepose.SetActive(true);
        if (uiCraft != null) uiCraft.SetActive(false);
    }

    void ShowCraftUI()
    {
        if (uiCraft != null) uiCraft.SetActive(true);
        if (uiDepose != null) uiDepose.SetActive(false);
    }

    void HideAllUI()
    {
        if (uiCraft != null) uiCraft.SetActive(false);
        if (uiDepose != null) uiDepose.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (other.CompareTag("Player"))
            playerWeaponManager = other.GetComponentInChildren<WeaponManager>();

        playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerWeaponManager = null;
            playerInRange = false;
            HideAllUI();
        }
    }
}
