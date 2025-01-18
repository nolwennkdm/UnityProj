using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    
    public int maxSlots = 8; // Nombre maximum de slots
    private List<Item> items = new List<Item>(); // Liste des objets

    public bool AddItem(Item item)
    {
        if (items.Count < maxSlots)
        {
            items.Add(item);
            return true;
        }
        //print inventaire plein ? 
        return false;
    }

    public List<Item> GetItems()
    {
        return items;
    }
}