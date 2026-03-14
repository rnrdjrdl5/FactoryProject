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

    public event Action<Item> OnClickItem;
    
    List<ItemList> itemLists = new();
    Tables.ItemType itemType;
    Bag bag;
    Inventory inventory;

    protected override void OnSetPanelDatas()
    {
        base.OnSetPanelDatas();
        
        bag = GetTargetPanelDatas<Bag>();
        inventory = bag.GetInventory(itemType);

        RefreshUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        
        itemLists.Clear();
        for (int i = 0; i < inventory.Items.Count; i+= lowCount)
        {
            itemLists.Add(ItemList.Create(inventory.Items.Skip(i).Take(lowCount)));
        }

        scroller.Delegate ??= this;
        scroller.ReloadData();
    }

    public void SetItemType(Tables.ItemType itemType)
    {
        this.itemType = itemType;
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
        OnClickItem?.Invoke(item);
    }
}
