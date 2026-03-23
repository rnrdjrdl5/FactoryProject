using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class UITeamLinePanelElement : PanelElement, IEnhancedScrollerDelegate
{
    [SerializeField] EnhancedScroller scroller;
    [SerializeField] float cellSize;
    [SerializeField] AllocGameObject allocGameObject;

    List<ExItem> itemLists = new();
    PlayerStorage playerStorage;

    public override void Initialize(Panel panel, IInitData initData = null)
    {
        base.Initialize(panel, initData);
        
        scroller.Delegate ??= this;
        scroller.ReloadData();
    }

    protected override void OnSetPanelDatas()
    {
        playerStorage = GetTargetPanelDatas<PlayerStorage>();
        RefreshUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        
        if (playerStorage == null)
        {
            return;
        }
        
        itemLists.Clear();
        for (int i = 0; i < playerStorage.PlayerItem.Count; i++)
        {
            var exItem = ExItem.Create(playerStorage.PlayerItem[i]);
            itemLists.Add(exItem);
        }
        
        scroller.ReloadData();
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        if (playerStorage == null)
        {
            return 0;
        }

        return playerStorage.PlayerItem.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return cellSize;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var cellObject = allocGameObject.AllocateObject();
        var cellView = cellObject.GetComponent<UITeamLineCellView>();
        var item = playerStorage.PlayerItem[dataIndex];
        cellView.UpdateItem(item, ClickItem);
        
        return cellView;
    }

    void ClickItem(Item item)
    {
        var msg = new UIMsg.SelectTeamLineItemMsg()
        {
            Item = item
        };
        
        Panel.MessageBus.Publish(msg);
    }
}

public static partial class UIMsg
{
    public struct SelectTeamLineItemMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.UI;
        public Item Item;
    }
}
