using System.Linq;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class UIItemListCellView : EnhancedScrollerCellView
{
    [SerializeField] AllocGameObject cellAllocator;
    
    public void SetItemList(ItemList itemList)
    {
        var itemCount = itemList.Items.Count();
        cellAllocator.DeallocateObjects();
        cellAllocator.AllocateObject(itemCount);

        var index = 0;
        foreach (var item in itemList.Items)
        {
            var cellObject = cellAllocator.AllocatedObjects[index];
            var uiItem = cellObject.GetComponent<UIItem>();
            uiItem.UpdateItemData(item);
            index++;
        }
    }
}