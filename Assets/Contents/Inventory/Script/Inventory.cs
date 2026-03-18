using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : IMessageBus
{
    public IReadOnlyList<Item> Items => items;
    public MessageBus MessageBus { get; set; }

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
            item = Item.Create(itemKey, amount);
            items.Add(item);

            return item;
        }
        
        item.Acquire(amount);
        
        MessageBus?.Publish(new EntityDataMsg.InventoryChangedMsg
        {
            Inventory = this
        });
        
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
        
        MessageBus?.Publish(new EntityDataMsg.InventoryChangedMsg
        {
            Inventory = this
        });
        
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
        
        MessageBus?.Publish(new EntityDataMsg.InventoryChangedMsg
        {
            Inventory = this
        });
    }

    public void Unequip(string itemKey)
    {
        var item = items.FirstOrDefault(item => item.ItemKey == itemKey);
        Unequip(item);
    }

    public void Unequip(Item item)
    {
        item.SetEquip(false);
        
        MessageBus?.Publish(new EntityDataMsg.InventoryChangedMsg
        {
            Inventory = this
        });
    }


    public static Inventory Create(Tables.ItemType itemType)
    {
        var inventory = new Inventory();
        inventory.Initialize(itemType);

        return inventory;
    }

    public void OnSetMessageBus()
    {
    }
}

public static partial class EntityDataMsg
{
    public struct InventoryChangedMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.EntityData;
        public Inventory Inventory;
    }
}
