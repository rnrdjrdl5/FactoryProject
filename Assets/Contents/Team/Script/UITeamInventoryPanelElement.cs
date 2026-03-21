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

    PlayerItemStorage playerItemStorage;
    List<ExItemList> itemLists = new();
    
    protected override void OnSetPanelDatas()
    {
        base.OnSetPanelDatas();
        playerItemStorage = GetTargetPanelDatas<PlayerItemStorage>();
        
        RefreshUI();
    }

    protected override void OnUnsetPanelDatas()
    {
        playerItemStorage = null;
        
        base.OnUnsetPanelDatas(); 
    }

    public override void RefreshUI()
    {
        base.RefreshUI();

        itemLists.Clear();
        if (playerItemStorage == null)
        {
            scroller.Delegate ??= this;
            scroller.ReloadData();
            return;
        }

        for (int i = 0; i < playerItemStorage.Items.Count; i += lowCount)
        {
            var exItems = playerItemStorage.Items
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
        var msg = new UIMsg.SelectTeamInventoryItemMsg
        {
            Item = item
        };

        Panel.MessageBus.Publish(msg);
    }
}

public static partial class UIMsg
{
    public struct SelectTeamInventoryItemMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.UI;
        public Item Item;
    }
}
