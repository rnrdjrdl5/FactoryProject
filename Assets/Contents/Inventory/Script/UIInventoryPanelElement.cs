using System;
using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class UIInventoryPanelElement : PanelElement, IEnhancedScrollerDelegate 
{
    [SerializeField] EnhancedScroller scroller;
    [SerializeField] float cellSize;
    [SerializeField] int lowCount;
    [SerializeField] AllocGameObject allocGameObject;

    List<Item> equipItemKeys = new();
    List<ExItemList> itemLists = new();
    Tables.ItemType itemType;
    Bag bag;
    Inventory inventory;

    protected override void OnSetPanelDatas()
    {
        base.OnSetPanelDatas();
        
        bag = GetTargetPanelDatas<Bag>();
        inventory = bag.GetInventory(itemType);

        if (inventory != null)
        {
            RefreshUI();
        }
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        
        itemLists.Clear();
        for (int i = 0; i < inventory.Items.Count; i+= lowCount)
        {
            var exItems = inventory.Items
                .Skip(i)
                .Take(lowCount)
                .Select(item => ExItem.Create(item, equipItemKeys.Contains(item)));
            itemLists.Add(ExItemList.Create(exItems));
        }

        scroller.Delegate ??= this;
        scroller.ReloadData();
    }

    public void SetItemType(Tables.ItemType itemType)
    {
        this.itemType = itemType;
    }

    public void SetEquipItemKeys(IEnumerable<Item> items)
    {
        equipItemKeys.Clear();
        if (items != null)
        {
            equipItemKeys.AddRange(items);
        }

        RefreshUI();
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
        var msg = new ClickInventoryItemMsg
        {
            Item = item
        };
        
        Panel.MessageBus.Publish(msg);
    }
}

public class ClickInventoryItemMsg
{
    public Item Item;
}
