using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class UITeamLinePanelElement : PanelElement, IEnhancedScrollerDelegate
{
    [SerializeField] EnhancedScroller scroller;
    [SerializeField] float cellSize;
    [SerializeField] AllocGameObject allocGameObject;
    Tables.ItemType itemType => Tables.ItemType.Player;
    Inventory targetInventory;

    protected override void OnSetPanelDatas()
    {
        var bag = GetTargetPanelDatas<Bag>();
        targetInventory = bag.GetInventory(itemType);
    }
    
    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return targetInventory.Items.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return cellSize;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var cellObject = allocGameObject.AllocateObject();
        var cellView = cellObject.GetComponent<UITeamLineCellView>();
        var item = targetInventory.Items[dataIndex];
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
