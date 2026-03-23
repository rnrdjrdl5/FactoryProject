using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Equipment : IEntityData, IMessageBus
{
    [JsonIgnore] public MessageBus MessageBus { get; set; }
    
    [JsonProperty] Dictionary<Tables.ItemType, long> equipItems = new();
    
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
        
        equipItems.Add(item.ItemData.itemType, item.UniqueId);
        item.SetEquip(true);
        
        MessageBus?.Publish(new EntityDataMsg.EquipmentEquipMsg
        {
            Equipment = this,
            Item = item
        });

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
        
        MessageBus?.Publish(new EntityDataMsg.UnequipmentEquipMsg
        {
            Equipment = this,
            Item = item
        });

        return true;
    }

    public bool TryGetEquipUid(Tables.ItemType itemType, out long itemUid)
    {
        return equipItems.TryGetValue(itemType, out itemUid);
    }
    
    public void OnSetMessageBus()
    {
        
    }
}

public static partial class EntityDataMsg
{
    public struct EquipmentEquipMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.EntityData;
        public Equipment Equipment;
        public Item Item;
    }

    public struct UnequipmentEquipMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.EntityData;
        public Equipment Equipment;
        public Item Item;
    }
}
