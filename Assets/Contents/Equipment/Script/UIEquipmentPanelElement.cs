using System.Collections.Generic;
using UnityEngine;
using Tables;

public class UIEquipmentPanelElement : PanelElement
{
    [System.Serializable]
    class Slot
    {
        public ItemSlotType itemSlotType;
        public UIItem ui;
        public bool clickable;
    }
    
    [SerializeField] List<Slot> slots = new();

    readonly Dictionary<ItemSlotType, Slot> slotMap = new();
    
    Bag storageBag;
    PlayerData playerLocalData;
    Item playerItem;

    public override void Initialize(Panel panel, IInitData initData = null)
    {
        base.Initialize(panel, initData);
        
        slotMap.Clear();
        for (var i = 0; i < slots.Count; i++)
        {
            var slot = slots[i];
            slotMap[slot.itemSlotType] = slot;
            
            if (slot.clickable)
            {
                slot.ui.SetClickEvent(ClickItem);
            }
        }

        RefreshUI();
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

            if (slot.itemSlotType == ItemSlotType.Player)
            {
                if (playerItem != null)
                {
                    slot.ui.gameObject.SetActive(true);
                    slot.ui.UpdateItemData(playerItem);
                }

                else
                {
                    slot.ui.UpdateEmptyItemData();
                }
            }
            else
            {
                var item = GetItem(slot);
                if (item != null)
                {
                    slot.ui.gameObject.SetActive(true);
                    slot.ui.UpdateItemData(item);
                }
                else
                {
                    slot.ui.UpdateEmptyItemData();
                }
            }
        }
    }

    Item GetItem(Slot slot)
    {
        if (playerLocalData == null)
        {
            return null;
        }

        if (!playerLocalData.Equipment.TryGetEquipUid(slot.itemSlotType, out var equipUid))
        {
            return null;
        }

        var inventory = storageBag.GetInventory(slot.itemSlotType);
        return !inventory.TryGetItemByItemUid(equipUid.UniqueId, out var playerItem) ? null : playerItem;
    }

    public void SetStorageBag(Bag bag)
    {
        storageBag = bag;
    }
    
    public void SetPlayerData(PlayerData data, Item playerItem)
    {
        if (playerLocalData != null)
        {
            var equipment = playerLocalData?.Equipment;
            if (equipment?.MessageBus != null)
            {
                equipment.MessageBus.Unsubscribe<EntityDataMsg.EquipmentEquipMsg>(EquipmentEquip);
                equipment.MessageBus.Unsubscribe<EntityDataMsg.UnequipmentEquipMsg>(UnequipmentEquip);
            }
        }

        this.playerItem = playerItem; 
        playerLocalData = data;

        if (playerLocalData != null)
        {
            var equipment = playerLocalData?.Equipment;
            if (equipment?.MessageBus != null)
            {
                equipment.MessageBus.Subscribe<EntityDataMsg.EquipmentEquipMsg>(EquipmentEquip);
                equipment.MessageBus.Subscribe<EntityDataMsg.UnequipmentEquipMsg>(UnequipmentEquip);
            }
        }
        
        RefreshUI();
    }

    void EquipmentEquip(EntityDataMsg.EquipmentEquipMsg msg)
    {
        var type = msg.Item.ItemData.itemSlotType;
        if (!slotMap.ContainsKey(type))
        {
            return;
        }
        
        RefreshUI();
    }
    
    void UnequipmentEquip(EntityDataMsg.UnequipmentEquipMsg msg)
    {
        var type = msg.Item.ItemData.itemSlotType;
        if (!slotMap.ContainsKey(type))
        {
            return;
        }
        
        RefreshUI();
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
}

public static partial class UIMsg
{
    public struct SelectEquipItemMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.UI;
        public Item Item;
    }
}

