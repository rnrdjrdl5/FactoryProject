using Tables;

public class EquipmentPopupProcessor : Processor
{
    EquipmentPopup equipmentPopup;
    UIEquipmentPanelElement uiEquipmentPanelElement;
    UIInventoryPanelElement uiInventoryPanelElement;
    PlayerStorage playerStorage;
    PlayerData targetPlayerData;
    Item selectedPlayer;
    Bag bag;
    
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
        playerStorage = equipmentPopup.GetTargetPanelDatas<PlayerStorage>();
        bag = equipmentPopup.GetTargetPanelDatas<Bag>();
        uiInventoryPanelElement.SetData(bag, ItemType.Weapon);
        
        uiEquipmentPanelElement.SetStorageBag(bag);
    }

    void OnUnsetPanelDatasAction()
    {
        playerStorage = null;
    }

    void SelectEquipItem(UIMsg.SelectEquipItemMsg msg)
    {
        if (targetPlayerData == null)
        {
            return;
        }
        
        targetPlayerData.Equipment.TryEquipItem(msg.Item);
    }

    void SelectTeamLineItem(UIMsg.SelectTeamLineItemMsg msg)
    {
        selectedPlayer = msg.Item;
        if (!playerStorage.TryGetPlayerDataByItemUid(msg.Item.UniqueId, out var nextPlayerData))
        {
            return;
        }

        uiEquipmentPanelElement.SetPlayerData(null, null);
        if (targetPlayerData != null)
        {
            targetPlayerData.Equipment.MessageBus.Unsubscribe<EntityDataMsg.EquipmentEquipMsg>(EquipmentEquip);
            targetPlayerData.Equipment.MessageBus.Unsubscribe<EntityDataMsg.UnequipmentEquipMsg>(UnequipmentEquip);
        }

        targetPlayerData = nextPlayerData;
        if (targetPlayerData != null)
        {
            targetPlayerData.Equipment.MessageBus.Subscribe<EntityDataMsg.EquipmentEquipMsg>(EquipmentEquip);
            targetPlayerData.Equipment.MessageBus.Subscribe<EntityDataMsg.UnequipmentEquipMsg>(UnequipmentEquip);
        }
        
        uiEquipmentPanelElement.SetPlayerData(targetPlayerData, msg.Item);
    }

    void SelectInventoryItem(UIMsg.SelectInventoryItemMsg msg)
    {
        if (targetPlayerData == null)
        {
            return;
        }
        
        targetPlayerData.Equipment.TryEquipItem(msg.Item);
    }
    
    void EquipmentEquip(EntityDataMsg.EquipmentEquipMsg msg)
    {
        uiEquipmentPanelElement.RefreshUI();
    }
    
    void UnequipmentEquip(EntityDataMsg.UnequipmentEquipMsg msg)
    {
        uiInventoryPanelElement.RefreshUI();
    }
}
