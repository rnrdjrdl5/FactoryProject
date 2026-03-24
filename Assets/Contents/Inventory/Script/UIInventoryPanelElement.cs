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

    protected Bag bag;
    protected Inventory inventory;
    
    List<ExItemList> itemLists = new();
    Tables.ItemType itemType;

    public override void Initialize(Panel panel, IInitData initData = null)
    {
        base.Initialize(panel, initData);
        
        scroller.Delegate ??= this;
        scroller.ReloadData();
    }

    public override void RefreshUI()
    {
        if (bag == null || inventory == null)
        {
            return;
        }
        
        base.RefreshUI();
        
        itemLists.Clear();
        for (int i = 0; i < inventory.Items.Count; i+= lowCount)
        {
            var exItems = inventory.Items
                .Skip(i)
                .Take(lowCount)
                .Select(ExItem.Create);
            itemLists.Add(ExItemList.Create(exItems));
        }
        
        scroller.ReloadData();
    }

    protected override void OnSetPanelDatas()
    {
        base.OnSetPanelDatas();

        bag = GetTargetPanelDatas<Bag>();
        if (bag != null)
        {
            inventory = bag.GetInventory(itemType);
        }

        RefreshUI(); 
    }

    protected override void OnUnsetPanelDatas()
    {
        bag = null;
        inventory = null;
        
        base.OnUnsetPanelDatas();
    }

    public void SetItemType(Tables.ItemType itemType)
    {
        this.itemType = itemType;
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
        var msg = new UIMsg.SelectInventoryItemMsg
        {
            Item = item
        };
        
        Panel.MessageBus.Publish(msg);
    }
}

public static partial class UIMsg
{
    public struct SelectInventoryItemMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.UI;
        public Item Item;
    }
}
