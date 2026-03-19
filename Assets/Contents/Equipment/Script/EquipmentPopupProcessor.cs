using Tables;

public class EquipmentPopupProcessor : Processor
{
    Inventory playerInventory;
    Equipment equipment;
    EquipmentPopup equipmentPopup;
    UIEquipmentPanelElement uiEquipmentPanelElement;
    UIInventoryPanelElement uiInventoryPanelElement;
    
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);
        
        equipmentPopup = Entity as EquipmentPopup;
        equipmentPopup.OnSetPanelDatas += OnSetPanelDatas;
        uiEquipmentPanelElement = equipmentPopup.GetPanelElement<UIEquipmentPanelElement>();
        uiInventoryPanelElement = equipmentPopup.GetPanelElement<UIInventoryPanelElement>();
    }

    public override void Ready()
    {
        base.Ready();
        
        equipmentPopup.MessageBus.Subscribe<UIMsg.SelectEquipItemMsg>(SelectEquipItem);
        equipmentPopup.MessageBus.Subscribe<UIMsg.SelectTeamLineItemMsg>(SelectTeamLineItem);
        equipmentPopup.MessageBus.Subscribe<UIMsg.SelectInventoryItemMsg>(SelectInventoryItem);
    }

    public override void Uninitialize()
    {
        equipmentPopup.OnSetPanelDatas -= OnSetPanelDatas;
        equipmentPopup.MessageBus.Unsubscribe<UIMsg.SelectEquipItemMsg>(SelectEquipItem);
        equipmentPopup.MessageBus.Unsubscribe<UIMsg.SelectTeamLineItemMsg>(SelectTeamLineItem);
        equipmentPopup.MessageBus.Unsubscribe<UIMsg.SelectInventoryItemMsg>(SelectInventoryItem);
        
        base.Uninitialize();
    }

    void OnSetPanelDatas()
    {
        equipment = equipmentPopup.GetTargetPanelDatas<Equipment>();
    }

    void SelectEquipItem(UIMsg.SelectEquipItemMsg msg)
    {
        equipment.TryUnequipItem(msg.Item);

        if (msg.Item.ItemData.itemType == ItemType.Weapon)
        {
            uiEquipmentPanelElement.SetWeaponItem(msg.Item);
        }
        
        else if (msg.Item.ItemData.itemType == ItemType.Armor)
        {
            uiEquipmentPanelElement.SetArmorItem(msg.Item);
        }
        
        else if (msg.Item.ItemData.itemType == ItemType.Accessory)
        {
            uiEquipmentPanelElement.SetAccessoryItem(msg.Item);
        }
    }

    void SelectTeamLineItem(UIMsg.SelectTeamLineItemMsg msg)
    {
        equipmentPopup.GetPanelElement<UIEquipmentPanelElement>();
        uiEquipmentPanelElement.SetPlayerItem(msg.Item);
    }

    void SelectInventoryItem(UIMsg.SelectInventoryItemMsg msg)
    {
        equipment.TryEquipItem(msg.Item);
        uiInventoryPanelElement.RefreshUI();
    }
}