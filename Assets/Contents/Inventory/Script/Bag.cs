using System;
using System.Collections.Generic;

public class Bag : IEntityData, IMessageBus
{
    public MessageBus MessageBus { get; set; }
    
    Dictionary<Tables.ItemType, Inventory> inventoryTab = new();
    
    public void Initialize(IInitData initData = null)
    {
        inventoryTab.Clear();
        foreach (var itemType in Tables.EnumLogic.ItemTypes)
        {
            var inventory = Inventory.Create(itemType);
            inventory.MessageBus = MessageBus;
            inventory.OnSetMessageBus();
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
    
    public void OnSetMessageBus()
    {
        foreach (var inventory in inventoryTab.Values)
        {
            inventory.MessageBus = MessageBus;
            inventory.OnSetMessageBus();
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
            inventory.MessageBus = MessageBus;
            inventory.OnSetMessageBus();
            inventoryTab.Add(item.itemType, inventory);
        }

        inventory.AddItem(item.Key,amount);
        
        MessageBus?.Publish(new EntityDataMsg.BagItemAddedMsg
        {
            Bag = this,
            Item = item,
            Amount = amount
        });
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
            MessageBus?.Publish(new EntityDataMsg.BagItemRemovedMsg
            {
                Bag = this,
                Item = item,
                Amount = amount
            });
        }

        return result;
    }
}

public static partial class EntityDataMsg
{
    public struct BagItemAddedMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.EntityData;
        public Bag Bag;
        public Tables.Item Item;
        public int Amount;
    }

    public struct BagItemRemovedMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.EntityData;
        public Bag Bag;
        public Tables.Item Item;
        public int Amount;
    }
}
