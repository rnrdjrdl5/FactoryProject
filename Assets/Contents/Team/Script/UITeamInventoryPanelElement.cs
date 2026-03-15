using System;
using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class UITeamInventoryPanelElement : UIInventoryPanelElement
{
    protected override void OnSetPanelDatas()
    {
        base.OnSetPanelDatas();
        SetItemType(Tables.ItemType.Animal);

        if (inventory != null)
        {
            ExternalMessageBus?.Subscribe<EntityDataMsg.InventoryChangedMsg>(OnInventoryChanged);
        }
    }

    protected override void OnUnsetPanelDatas()
    {
        if (inventory != null)
        {
            ExternalMessageBus?.Unsubscribe<EntityDataMsg.InventoryChangedMsg>(OnInventoryChanged);
        }
        
        base.OnUnsetPanelDatas(); 
    }

    void OnInventoryChanged(EntityDataMsg.InventoryChangedMsg msg)
    {
        if (msg.Inventory != inventory)
            return;

        RefreshUI();
    }
}
