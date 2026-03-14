using System;
using System.Collections.Generic;

public class Bag : IEntityData
{
    public event Action OnChanged;
    
    Dictionary<Tables.ItemType, Inventory> inventoryTab = new();
    
    public void Initialize(IInitData initData = null)
    {
        inventoryTab.Clear();
        foreach (var itemType in Tables.Item.ItemTypes)
        {
            var inventory = new Inventory();
            inventory.Initialize(itemType);
            inventoryTab.Add(itemType, inventory);
        }
    }

    public void Uninitialize()
    {
        foreach (var inventory in inventoryTab.Values)
        {
            inventory.Uninitialize();
        }
    }

    public Inventory GetInventory(Tables.ItemType itemType)
    {
        return inventoryTab.GetValueOrDefault(itemType);
    }

    public void AddItem(Tables.Item item, int amount)
    {
        if (!inventoryTab.TryGetValue(item.itemType, out var inventory))
        {
            inventory = Inventory.Create(item.itemType);
            inventoryTab.Add(item.itemType, inventory);
        }

        inventory.AddItem(item.Key,amount);
        OnChanged?.Invoke();
    }

    public void AddItem(string itemKey, int amount)
    {
        var item = Tables.Item.Get(itemKey);
        AddItem(item, amount);
    }

    public bool TryRemoveItem(Tables.Item item, int amount)
    {
        if (!inventoryTab.TryGetValue(item.itemType, out var inventory))
        {
            return false;
        }
        
        var result = inventory.TryRemoveItem(item.Key, amount);
        if (result)
        {
            OnChanged?.Invoke();
        }

        return result;
    }
}
