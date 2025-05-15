using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class InventoryUI : MonoBehaviour
{
    public Inventory inventory; // Référence à l’inventaire du joueur
    public GameObject slotPrefab; // Prefab du slot UI
    public Transform slotParent; // Parent des slots dans l’UI
    public WeaponManager weaponManager;


    void Start()
    {
        // je fais ca car le joueur est instancié dynamiquement 
        if (weaponManager == null)
        {
            weaponManager = FindAnyObjectByType<WeaponManager>();
            Debug.Log($"🔧 WeaponManager récupéré dynamiquement : {weaponManager}");
        }

        InitUI();
    }


    void InitUI()
    {
        for (int i = 0; i < inventory.inventorySize; i++)
        {
            int index = i; // capture propre ( par référence ca ne fonctionne pas)
            GameObject slot = Instantiate(slotPrefab, slotParent);
            slot.GetComponent<Button>().onClick.AddListener(() => SelectItem(index));
        }
    }

    public void UpdateUI()
    {
        for (int i = 0; i < inventory.inventorySize; i++)
        {
            Transform slot = slotParent.GetChild(i);
            Image icon = slot.GetComponentInChildren<Image>();
            
            if (inventory.inventorySlots[i] != null)
            {
                icon.sprite = inventory.inventorySlots[i].icon;
                icon.enabled = true;
                icon.color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }

    void SelectItem(int slotIndex)
    {
        Item item = inventory.inventorySlots[slotIndex];

        if (item != null)
        {
            Debug.Log($"🎯 Slot {slotIndex} contient : {item.itemName}");

            if (weaponManager != null && item.weaponPrefab != null)
            {
                int index = weaponManager.weaponPrefabs.IndexOf(item.weaponPrefab);
                if (index != -1)
                {
                    Debug.Log($"🔁 weaponManager.weaponPrefabs.Count = {weaponManager.weaponPrefabs.Count}");
                    Debug.Log($"📦 Index trouvé = {index}");

                    weaponManager.EquipWeapon(index);
                    Debug.Log($"✅ Arme équipée : {item.itemName} (Index {index})");
                }
                else
                {
                    Debug.LogWarning($"❌ {item.itemName} non trouvé dans WeaponManager.weaponPrefabs !");
                }
            }
        }
        else
        {
            Debug.Log("🛑 Slot vide → déséquipement");

            if (weaponManager != null)
                weaponManager.UnEquipWeapon();
        }
    }


}
