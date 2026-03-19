using System.Collections.Generic;
using UnityEngine;
using Tables;

public class UIEquipmentPanelElement : PanelElement
{
    [System.Serializable]
    class Slot
    {
        public ItemType type;
        public UIItem ui;
        public bool clickable;
    }
    
    [SerializeField] List<Slot> slots = new();

    readonly Dictionary<ItemType, Slot> slotMap = new();
    readonly Dictionary<ItemType, Item> itemMap = new();
    
    Equipment equipment;
    
    protected override void OnSetPanelDatas()
    {
        base.OnSetPanelDatas();

        equipment = GetTargetPanelDatas<Equipment>();
        if (equipment?.MessageBus != null)
        {
            equipment.MessageBus.Subscribe<EntityDataMsg.EquipmentEquipMsg>(EquipmentEquip);
            equipment.MessageBus.Subscribe<EntityDataMsg.UnequipmentEquipMsg>(UnequipmentEquip);
        }
    }

    protected override void OnUnsetPanelDatas()
    {
        if (equipment != null && equipment.MessageBus != null)
        {
            equipment.MessageBus.Unsubscribe<EntityDataMsg.EquipmentEquipMsg>(EquipmentEquip);
            equipment.MessageBus.Unsubscribe<EntityDataMsg.UnequipmentEquipMsg>(UnequipmentEquip);
        }
        
        base.OnUnsetPanelDatas();
    }

    public override void Initialize(Panel panel, IInitData initData = null)
    {
        base.Initialize(panel, initData);

        slotMap.Clear();
        for (var i = 0; i < slots.Count; i++)
        {
            var slot = slots[i];
            slotMap[slot.type] = slot;
            
            if (slot.clickable)
            {
                slot.ui.SetClickEvent(ClickItem);
            }
        }
    }

    public override void Uninitialize()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            var slot = slots[i];
            if (slot.clickable)
            {
                slot.ui.SetClickEvent(null);
            }
        }
        
        base.Uninitialize();
    }

    void ClickItem(Item item)
    {
        if (!item.ItemData.CanEquip())
        {
            return;
        }

        var msg = new UIMsg.SelectEquipItemMsg()
        {
            Item = item
        };

        Panel.MessageBus.Publish(msg);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();

        for (int i = 0; i < slots.Count; i++)
        {
            var slot = slots[i];
            if (slot == null || slot.ui == null)
            {
                continue;
            }

            itemMap.TryGetValue(slot.type, out var item);
            
            if (item != null)
            {
                slot.ui.UpdateItemData(item);
            }
        }
    }

    public void SetEquip(Item item)
    {
        itemMap[item.ItemData.itemType] = item;
        RefreshUI();
    }

    void EquipmentEquip(EntityDataMsg.EquipmentEquipMsg msg)
    {
        var type = msg.Item.ItemData.itemType;
        if (!slotMap.ContainsKey(type))
        {
            return;
        }

        itemMap[type] = msg.Item;
        RefreshUI();
    }
    
    void UnequipmentEquip(EntityDataMsg.UnequipmentEquipMsg msg)
    {
        var type = msg.Item.ItemData.itemType;
        if (!slotMap.ContainsKey(type))
        {
            return;
        }

        itemMap[type] = null;
        RefreshUI();
    }
}

public static partial class UIMsg
{
    public struct SelectEquipItemMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.UI;
        public Item Item;
    }
}


