using System.Collections.Generic;
using Newtonsoft.Json;

public class Equipment : IEntityData, IMessageBus
{
    [JsonIgnore] public MessageBus MessageBus { get; set; }
    [JsonIgnore] public IEnumerable<Item> EquipItems => equipItems.Values;
    
    [JsonProperty] Dictionary<Tables.ItemSlotType, Item> equipItems = new();
    
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

        equipItems.TryGetValue(item.ItemData.itemSlotType, out var equipedItem);
        TryUnequipItem(equipedItem);
        
        equipItems[item.ItemData.itemSlotType] = item;
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
        if (item == null || !equipItems.ContainsKey(item.ItemData.itemSlotType))
        {
            return false;
        }

        equipItems.Remove(item.ItemData.itemSlotType);
        item.SetEquip(false);
        
        MessageBus?.Publish(new EntityDataMsg.UnequipmentEquipMsg
        {
            Equipment = this,
            Item = item
        });

        return true;
    }

    public bool TryGetEquipUid(Tables.ItemSlotType itemSlotType, out Item item)
    {
        return equipItems.TryGetValue(itemSlotType, out item);
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
