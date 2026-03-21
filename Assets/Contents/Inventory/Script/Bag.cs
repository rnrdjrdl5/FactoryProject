using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Bag : IEntityData, IMessageBus
{
    [JsonIgnore] public MessageBus MessageBus { get; set; }
    
    [JsonProperty] Dictionary<Tables.ItemType, Inventory> inventoryTab = new();
    
    public void Initialize(IInitData initData = null)
    {
        inventoryTab.Clear();
        foreach (var itemType in Tables.EnumLogic.ItemTypes)
        {
            var inventory = Inventory.Create(itemType);
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

    public void AddItem(Item item)
    {
        if (!inventoryTab.TryGetValue(item.ItemData.itemType, out var inventory))
        {
            inventory = Inventory.Create(item.ItemData.itemType);
            inventory.MessageBus = MessageBus;
            inventory.OnSetMessageBus();
            inventoryTab.Add(item.ItemData.itemType, inventory);
        }

        inventory.AddItem(item);
        
        MessageBus?.Publish(new EntityDataMsg.BagItemAddedMsg
        {
            Bag = this,
            Item = item,
        });
    }

    public bool TryRemoveItem(Item item, int amount)
    {
        if (!inventoryTab.TryGetValue(item.ItemData.itemType, out var inventory))
        {
            return false;
        }
        
        var result = inventory.TryRemoveItem(item.ItemData.Key, amount);
        if (result)
        {
            MessageBus?.Publish(new EntityDataMsg.BagItemRemovedMsg
            {
                Bag = this,
                Item = item,
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
        public Item Item;
    }

    public struct BagItemRemovedMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.EntityData;
        public Bag Bag;
        public Item Item;
    }
}
