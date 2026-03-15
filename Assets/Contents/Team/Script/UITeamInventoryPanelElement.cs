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
            ExternalMessageBus?.Subscribe<InventoryChangedMsg>(OnInventoryChanged);
        }
    }

    protected override void OnUnsetPanelDatas()
    {
        if (inventory != null)
        {
            ExternalMessageBus?.Unsubscribe<InventoryChangedMsg>(OnInventoryChanged);
        }
        
        base.OnUnsetPanelDatas(); 
    }

    void OnInventoryChanged(InventoryChangedMsg msg)
    {
        if (msg.Inventory != inventory)
            return;

        RefreshUI();
    }
}
