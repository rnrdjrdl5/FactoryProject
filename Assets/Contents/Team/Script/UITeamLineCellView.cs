using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class UITeamLineCellView : EnhancedScrollerCellView
{
    [SerializeField] UIItem uiItem;

    public void UpdateItem(Item item, System.Action<Item> onClick)
    {
        uiItem.UpdateItemData(item);
        uiItem.SetClickEvent(onClick);
    }
}
