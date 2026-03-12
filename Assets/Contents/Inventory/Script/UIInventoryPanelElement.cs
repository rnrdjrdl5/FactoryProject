using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class UIInventoryPanelElement : PanelElement, IEnhancedScrollerDelegate 
{
    [SerializeField] EnhancedScroller scroller;
    [SerializeField] GameObject cellPrefab;
    [SerializeField] float cellSize;
    [SerializeField] int lowCount;
    
    ObjectPoolAbility objectPoolAbility;
    Tables.ItemType itemType;
    Bag bag;
    Inventory inventory;

    List<ItemList> itemLists = new(); 
    
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        objectPoolAbility = Entry.RootRealm.GetAbility<ObjectPoolAbility>();
    }

    protected override void OnSetPanelDatas()
    {
        base.OnSetPanelDatas();
        
        bag = GetTargetPanelDatas<Bag>();
        inventory = bag.GetInventory(itemType);
        
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
        var cellObject = objectPoolAbility.AllocateGameObject(cellPrefab, transform);
        var cellView = cellObject.GetComponent<UIItemListCellView>();
        var itemList = itemLists[dataIndex];
        cellView.SetItemList(itemList);

        return cellView;
    }
}
