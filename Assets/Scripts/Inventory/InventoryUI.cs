using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory; // Référence à l’inventaire du joueur
    public GameObject slotPrefab; // Prefab du slot UI
    public Transform slotParent; // Parent des slots dans l’UI

    void Start()
    {
        for (int i = 0; i < inventory.inventorySize; i++)
        {
            GameObject slot = Instantiate(slotPrefab, slotParent);
            slot.GetComponent<Button>().onClick.AddListener(() => SelectItem(i));
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
            }
            else
            {
                icon.sprite = null;
                icon.enabled = false;
            }
        }
    }

    void SelectItem(int slotIndex)
    {
        Debug.Log("Item sélectionné : " + (inventory.inventorySlots[slotIndex] != null ? inventory.inventorySlots[slotIndex].itemName : "Vide"));
    }
}
