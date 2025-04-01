using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName; // Nom de l'objet
    public Sprite icon; // Icône de l'objet pour l'UI
    public int maxStack = 64; // Nombre max d’objets empilables
}