using System;
using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class UITeamInventoryPanelElement : PanelElement, IEnhancedScrollerDelegate
{
    [SerializeField] EnhancedScroller scroller;
    [SerializeField] float cellSize;
    [SerializeField] int lowCount;
    [SerializeField] AllocGameObject allocGameObject;

    TeamInventory teamInventory;
    Inventory inventory;
    List<ExItemList> itemLists = new();
    
    protected override void OnSetPanelDatas()
    {
        base.OnSetPanelDatas();
        teamInventory = GetTargetPanelDatas<TeamInventory>();
        inventory = teamInventory?.Inventory; 
        
        RefreshUI();
    }

    protected override void OnUnsetPanelDatas()
    {
        inventory = null;
        teamInventory = null;
        
        base.OnUnsetPanelDatas(); 
    }

    public override void RefreshUI()
    {
        base.RefreshUI();

        itemLists.Clear();
        if (inventory == null)
        {
            scroller.Delegate ??= this;
            scroller.ReloadData();
            return;
        }

        for (int i = 0; i < inventory.Items.Count; i += lowCount)
        {
            var exItems = inventory.Items
                .Skip(i)
                .Take(lowCount)
                .Select(ExItem.Create);
            itemLists.Add(ExItemList.Create(exItems));
        }

        scroller.Delegate ??= this;
        scroller.ReloadData();
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return itemLists.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return cellSize;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var cellObject = allocGameObject.AllocateObject();
        var cellView = cellObject.GetComponent<UIItemListCellView>();
        var itemList = itemLists[dataIndex];
        cellView.SetItem(itemList, ClickItem);

        return cellView;
    }

    void ClickItem(Item item)
    {
        var msg = new UIMsg.SelectInventoryItemMsg
        {
            Item = item
        };

        Panel.MessageBus.Publish(msg);
    }
}
