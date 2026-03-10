using System.Linq;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class UIItemListCellView : EnhancedScrollerCellView
{
    [SerializeField] AllocGameObject cellAllocator;
    
    public void SetItemList(ItemList itemList)
    {
        var itemCount = itemList.ItemKeys.Count();
        cellAllocator.DeallocateObjects();
        cellAllocator.AllocateObject(itemCount);

        var index = 0;
        foreach (var itemKey in itemList.ItemKeys)
        {
            var itemData = Tables.Item.Get(itemKey);
            var cellObject = cellAllocator.AllocatedObjects[index];
            var uiItem = cellObject.GetComponent<UIItem>();
            uiItem.UpdateItemData(itemData, itemData, itemData);
            index++;
        }
    }
}