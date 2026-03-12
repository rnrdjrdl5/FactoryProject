using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory
{
    public IReadOnlyList<Item> Items => items;

    List<Item> items = new();
    Tables.ItemType itemType;
    
    public void Initialize(Tables.ItemType itemType)
    {
        this.itemType = itemType;
        items.Clear();
    }

    public void Uninitialize()
    {
        
    }
    
    public Item AddItem(string itemKey, int amount)
    {
        var item = items.FirstOrDefault(item => item.ItemKey == itemKey && !item.IsFull());
        if (item == null)
        {
            item = new Item(itemKey, amount);
            items.Add(item);

            return item;
        }
        
        item.Acquire(amount);
        
        return item;
    }

    public bool TryRemoveItem(string itemKey, int amount)
    {
        var item = items.FirstOrDefault(item => item.ItemKey == itemKey && !item.IsEmpty());
        if (item == null)
        {
            return false;
        }

        item.Spend(amount);
        
        return true;
    }
}
