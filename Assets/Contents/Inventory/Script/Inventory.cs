using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory
{
    public IReadOnlyList<Item> Items => items;
    public event Action OnChanged;

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
        OnChanged?.Invoke();
        
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
        OnChanged?.Invoke();
        
        return true;
    }

    public void Equip(string itemKey)
    {
        var item = items.FirstOrDefault(item => item.ItemKey == itemKey);
        Equip(item);
    }

    public void Equip(Item item)
    {
        item.SetEquip(true);
        OnChanged?.Invoke();
    }

    public void Unequip(string itemKey)
    {
        var item = items.FirstOrDefault(item => item.ItemKey == itemKey);
        Unequip(item);
    }

    public void Unequip(Item item)
    {
        item.SetEquip(false);
        OnChanged?.Invoke();
    }


    public static Inventory Create(Tables.ItemType itemType)
    {
        var inventory = new Inventory();
        inventory.Initialize(itemType);

        return inventory;
    }
}
