using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public IReadOnlyDictionary<string, int> Items => items;
    public IEnumerable<string> ItemKeys => Items.Keys;

    Dictionary<string, int> items = new();
    Tables.ItemType itemType;
    
    public void Initialize(Tables.ItemType itemType)
    {
        this.itemType = itemType;
        items.Clear();
    }

    public void Uninitialize()
    {
        
    }

    public void AddItem(string itemKey, int amount)
    {
        if (!items.ContainsKey(itemKey))
        {
            items.Add(itemKey, amount);
            return;
        }
        
        items[itemKey] += amount;
    }

    public bool TryRemoveItem(string itemKey, int amount)
    {
        if (!items.TryGetValue(itemKey, out var leftAmount))
        {
            return false;
        }

        leftAmount = Mathf.Max(0, leftAmount - amount);
        items[itemKey] = leftAmount;

        return true;
    }
}
