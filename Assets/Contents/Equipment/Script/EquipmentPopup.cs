using UnityEngine;
public class EquipmentPopup : Panel , IPanelOrderType
{
    public static string PrefabPath = $"Equipment/{nameof(EquipmentPopup)}";
    public PanelOrderType PanelOrderType { get; set; } = PanelOrderType.Popup;

    Equipment equipment;
    UIInventoryPanelElement uiInventoryPanelElement;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        uiInventoryPanelElement = GetPanelElement<UIInventoryPanelElement>();
    }

    protected override void OnSetPanelDatas()
    {
        base.OnSetPanelDatas();

        equipment = GetTargetPanelDatas<Equipment>();
        equipment.MessageBus.Subscribe<EntityDataMsg.EquipmentEquipMsg>(EquipmentEquip);
        equipment.MessageBus.Subscribe<EntityDataMsg.UnequipmentEquipMsg>(UnequipmentEquip);
    }

    protected override void OnUnsetPanelDatas()
    {
        equipment.MessageBus.Unsubscribe<EntityDataMsg.EquipmentEquipMsg>(EquipmentEquip);
        equipment.MessageBus.Unsubscribe<EntityDataMsg.UnequipmentEquipMsg>(UnequipmentEquip);
        
        base.OnUnsetPanelDatas();
    }

    void EquipmentEquip(EntityDataMsg.EquipmentEquipMsg msg)
    {
        uiInventoryPanelElement.RefreshUI();
    }
    
    void UnequipmentEquip(EntityDataMsg.UnequipmentEquipMsg msg)
    {
        uiInventoryPanelElement.RefreshUI();
    }
}
