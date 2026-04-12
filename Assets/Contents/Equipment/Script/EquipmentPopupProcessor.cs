using Tables;


// NOTE : model에서는 Bow - Shield가 같은 part로 취급한다. ( 장착-해제 시 교체된다. )
// 정의한 기획에는 Bow - Shield는 동시 장착이 된다. ( MainWeapon / SubWeaponm )
// 결정 필요 : Bow - Shield 동시 장착 가능하게 할지? 
// -> 1. 양손무기 개념을 추가한다
// -> 2. 모델 규칙을 바꾼다.
public class EquipmentPopupProcessor : Processor
{
    EquipmentPopup equipmentPopup;
    UIEquipmentPanelElement uiEquipmentPanelElement;
    UIInventoryPanelElement uiInventoryPanelElement;
    UIStatPanelElement uiStatPanelElement;
    PlayerStorage playerStorage;
    PlayerData targetPlayerData;
    Item selectedPlayer;
    Bag bag;
    ItemSlotType activateTabType = ItemSlotType.MainWeapon;
    
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);
        
        equipmentPopup = Entity as EquipmentPopup;
        equipmentPopup.OnSetPanelDatasAction += OnSetPanelDatasAction;
        equipmentPopup.OnUnsetPanelDatasAction += OnUnsetPanelDatasAction;
        
        uiEquipmentPanelElement = equipmentPopup.GetPanelElement<UIEquipmentPanelElement>();
        uiInventoryPanelElement = equipmentPopup.GetPanelElement<UIInventoryPanelElement>();
        uiStatPanelElement = equipmentPopup.GetPanelElement<UIStatPanelElement>();
    }

    public override void Ready()
    {
        base.Ready();
        
        equipmentPopup.MessageBus.Subscribe<UIMsg.SelectEquipItemMsg>(SelectEquipItem);
        equipmentPopup.MessageBus.Subscribe<UIMsg.SelectTeamLineItemMsg>(SelectTeamLineItem);
        equipmentPopup.MessageBus.Subscribe<UIMsg.SelectInventoryItemMsg>(SelectInventoryItem);
        equipmentPopup.MessageBus.Subscribe<UIMsg.InventoryChangeTabMsg>(OnInventoryChangeTabMsg);
    }
    
    public override void Uninitialize()
    {
        equipmentPopup.OnSetPanelDatasAction -= OnSetPanelDatasAction;
        equipmentPopup.OnUnsetPanelDatasAction -= OnUnsetPanelDatasAction;
        equipmentPopup.MessageBus.Unsubscribe<UIMsg.SelectEquipItemMsg>(SelectEquipItem);
        equipmentPopup.MessageBus.Unsubscribe<UIMsg.SelectTeamLineItemMsg>(SelectTeamLineItem);
        equipmentPopup.MessageBus.Unsubscribe<UIMsg.SelectInventoryItemMsg>(SelectInventoryItem);
        equipmentPopup.MessageBus.Unsubscribe<UIMsg.InventoryChangeTabMsg>(OnInventoryChangeTabMsg);
        
        base.Uninitialize();
    }
    
    void OnSetPanelDatasAction()
    {
        playerStorage = equipmentPopup.GetTargetPanelDatas<PlayerStorage>();
        bag = equipmentPopup.GetTargetPanelDatas<Bag>();
        activateTabType = ItemSlotType.MainWeapon;
        
        uiEquipmentPanelElement.SetStorageBag(bag);
        RefreshInventoryPanel();
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
        uiStatPanelElement.SetTargetPanelDatas(new []{targetPlayerData});
    }

    void SelectInventoryItem(UIMsg.SelectInventoryItemMsg msg)
    {
        if (targetPlayerData == null)
        {
            return;
        }
        
        targetPlayerData.Equipment.TryEquipItem(msg.Item);
    }

    void OnInventoryChangeTabMsg(UIMsg.InventoryChangeTabMsg msg)
    {
        activateTabType = msg.ItemSlotType;
        RefreshInventoryPanel();
    }

    void RefreshInventoryPanel()
    {
        uiInventoryPanelElement.SetItemType(activateTabType);
    }
    
    void EquipmentEquip(EntityDataMsg.EquipmentEquipMsg msg)
    {
        uiEquipmentPanelElement.RefreshUI();
        uiInventoryPanelElement.RefreshUI();
    }
    
    void UnequipmentEquip(EntityDataMsg.UnequipmentEquipMsg msg)
    {
        uiEquipmentPanelElement.RefreshUI();
        uiInventoryPanelElement.RefreshUI();
    }
}
