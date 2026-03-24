using Tables;
using UnityEngine;

public class InventoryProcessor : Processor
{
    UIInventoryTabPanelElement uiInventoryTabPanelElement;
    UIInventoryPanelElement uiInventoryPanelElement;
    Panel panel;

    Tables.ItemType activateTabType;
    Inventory targetInventory;
    Bag bag;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);
        panel = Entity as Panel;

        uiInventoryTabPanelElement = panel.GetPanelElement<UIInventoryTabPanelElement>();
        uiInventoryPanelElement = panel.GetPanelElement<UIInventoryPanelElement>();
        
        panel.MessageBus.Subscribe<UIMsg.InventoryChangeTabMsg>(OnInventoryChangeTabMsg);
        panel.OnSetPanelDatasAction += OnSetPanelData;
        
        activateTabType = ItemType.Weapon;
    }

    public override void Uninitialize()
    {
        panel.MessageBus.Unsubscribe<UIMsg.InventoryChangeTabMsg>(OnInventoryChangeTabMsg);
        panel.OnSetPanelDatasAction -= OnSetPanelData;
        
        base.Uninitialize();
    }

    void OnSetPanelData()
    {
        bag = panel.GetTargetPanelDatas<Bag>();
        RefreshInventoryPanel();
    }

    void OnInventoryChangeTabMsg(UIMsg.InventoryChangeTabMsg msg)
    {
        activateTabType = msg.ItemType;
        RefreshInventoryPanel();
    }

    void RefreshInventoryPanel()
    {
        uiInventoryPanelElement.SetData(bag, activateTabType);
    }
}
