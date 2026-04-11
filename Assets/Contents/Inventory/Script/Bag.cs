using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Bag : IEntityData, IMessageBus
{
    [JsonIgnore] public MessageBus MessageBus { get; set; }
    
    [JsonProperty] Dictionary<Tables.ItemSlotType, Inventory> inventoryTab = new();
    
    public void Initialize(IInitData initData = null)
    {
        inventoryTab.Clear();
        foreach (var itemType in Tables.EnumLogic.ItemSlotTypes)
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

    public Inventory GetInventory(Tables.ItemSlotType itemSlotType)
    {
        return inventoryTab.GetValueOrDefault(itemSlotType);
    }

    public void AddItem(Item item)
    {
        if (!inventoryTab.TryGetValue(item.ItemData.itemSlotType, out var inventory))
        {
            inventory = Inventory.Create(item.ItemData.itemSlotType);
            inventory.MessageBus = MessageBus;
            inventory.OnSetMessageBus();
            inventoryTab.Add(item.ItemData.itemSlotType, inventory);
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
        if (!inventoryTab.TryGetValue(item.ItemData.itemSlotType, out var inventory))
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
