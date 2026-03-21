using Tables;

public class EquipmentPopupProcessor : Processor
{
    Inventory playerInventory;
    Equipment equipment;
    EquipmentPopup equipmentPopup;
    UIEquipmentPanelElement uiEquipmentPanelElement;
    UIInventoryPanelElement uiInventoryPanelElement;

    Item selectedPlayer;
    
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);
        
        equipmentPopup = Entity as EquipmentPopup;
        equipmentPopup.OnSetPanelDatasAction += OnSetPanelDatasAction;
        equipmentPopup.OnUnsetPanelDatasAction += OnUnsetPanelDatasAction;
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
        equipmentPopup.OnSetPanelDatasAction -= OnSetPanelDatasAction;
        equipmentPopup.OnUnsetPanelDatasAction -= OnUnsetPanelDatasAction;
        equipmentPopup.MessageBus.Unsubscribe<UIMsg.SelectEquipItemMsg>(SelectEquipItem);
        equipmentPopup.MessageBus.Unsubscribe<UIMsg.SelectTeamLineItemMsg>(SelectTeamLineItem);
        equipmentPopup.MessageBus.Unsubscribe<UIMsg.SelectInventoryItemMsg>(SelectInventoryItem);
        
        base.Uninitialize();
    }
    
    void OnSetPanelDatasAction()
    {
        var playerData = equipmentPopup.GetTargetPanelDatas<PlayerData>();
        equipment = playerData?.Equipment;
        if (equipment?.MessageBus != null)
        {
            equipment.MessageBus.Subscribe<EntityDataMsg.EquipmentEquipMsg>(EquipmentEquip);
            equipment.MessageBus.Subscribe<EntityDataMsg.UnequipmentEquipMsg>(UnequipmentEquip);
        }
    }

    void OnUnsetPanelDatasAction()
    {
        if (equipment?.MessageBus != null)
        {
            equipment.MessageBus.Unsubscribe<EntityDataMsg.EquipmentEquipMsg>(EquipmentEquip);
            equipment.MessageBus.Unsubscribe<EntityDataMsg.UnequipmentEquipMsg>(UnequipmentEquip);
        }
    }
    
    void EquipmentEquip(EntityDataMsg.EquipmentEquipMsg msg)
    {
        uiInventoryPanelElement.RefreshUI();
    }
    
    void UnequipmentEquip(EntityDataMsg.UnequipmentEquipMsg msg)
    {
        uiInventoryPanelElement.RefreshUI();
    }

    void SelectEquipItem(UIMsg.SelectEquipItemMsg msg)
    {
        equipment.TryUnequipItem(msg.Item);
    }

    void SelectTeamLineItem(UIMsg.SelectTeamLineItemMsg msg)
    {
        selectedPlayer = msg.Item;
        uiEquipmentPanelElement.SetEquip(msg.Item);
    }

    void SelectInventoryItem(UIMsg.SelectInventoryItemMsg msg)
    {
        equipment.TryEquipItem(msg.Item);
    }
}
