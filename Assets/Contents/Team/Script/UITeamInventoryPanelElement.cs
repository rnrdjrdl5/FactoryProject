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
            inventory.OnChanged += RefreshUI;
        }
    }

    protected override void OnUnsetPanelDatas()
    {
        if (inventory != null)
        {
            inventory.OnChanged -= RefreshUI;
        }
        
        base.OnUnsetPanelDatas(); 
    }
}