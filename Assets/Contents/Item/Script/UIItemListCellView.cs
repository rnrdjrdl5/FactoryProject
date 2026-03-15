using System;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class UIItemListCellView : EnhancedScrollerCellView
{
    [SerializeField] AllocGameObject cellAllocator;
    
    public void SetItem(ExItemList exItemList, Action<Item> OnClickItem)
    {
        var itemCount = exItemList.ExItems.Count();
        cellAllocator.DeallocateObjects();
        cellAllocator.AllocateObject(itemCount);

        var index = 0;
        foreach (var item in exItemList.ExItems)
        {
            var cellObject = cellAllocator.AllocatedObjects[index];
            var uiItem = cellObject.GetComponent<UIItem>();
            uiItem.UpdateItemData(item.Item);
            uiItem.SetClickEvent(OnClickItem);
            index++;
        }
    }
}
