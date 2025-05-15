using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int inventorySize = 36;
    public List<Item> inventorySlots = new List<Item>();

    void Start()
    {
        inventorySlots.Clear(); //on clean la liste
        for (int i = 0; i < inventorySize; i++)
        {
            inventorySlots.Add(null);
        }
        //on alloue dynamiquement les slots à nulls
        Debug.Log($"✅ Inventaire initialisé avec {inventorySlots.Count} slots.");
    }


    public bool AddItem(Item newItem)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i] == null)
            {
                inventorySlots[i] = newItem;
                return true;
            }
        }
        return false; // Inventaire plein
    }

    public void RemoveItem(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < inventorySlots.Count)
        {
            inventorySlots[slotIndex] = null;
        }
    }
}
