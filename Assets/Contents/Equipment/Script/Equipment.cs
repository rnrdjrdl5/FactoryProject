using System;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : IEntityData
{
    public event Action<Item> OnEquip;
    public event Action<Item> OnUnequip;
    
    Dictionary<Tables.ItemType, string> equipItems = new();
    
    public void Initialize(IInitData initData = null)
    {
        
    }

    public void Uninitialize()
    {
        
    }

    public bool TryEquipItem(Item item)
    {
        var result = Tables.Item.TryCanEquip(item.ItemKey, out var canEquip);
        if (!result || !canEquip)
        {
            return false;
        }

        TryUnequipItem(item);
        
        equipItems.Add(item.ItemData.itemType, item.ItemKey);
        item.SetEquip(true);
        OnEquip?.Invoke(item);

        return true;
    }

    public bool TryUnequipItem(Item item)
    {
        if (!equipItems.ContainsKey(item.ItemData.itemType))
        {
            return false;
        }

        equipItems.Remove(item.ItemData.itemType);
        item.SetEquip(false);
        OnUnequip?.Invoke(item);

        return true;
    }
}
