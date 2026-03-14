using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

// NOTE : Player가 Drop하는 플레이어 개체는 Item으로 취급한다
public class UITeamPanelElement : PanelElement, IEnhancedScrollerDelegate 
{
    [SerializeField] EnhancedScroller scroller;
    [SerializeField] float cellSize;
    [SerializeField] int lowCount;
    [SerializeField] AllocGameObject allocGameObject;

    Tables.ItemType itemType = Tables.ItemType.Animal;
    Bag bag;
    Inventory inventory;

    List<ItemList> teamLists = new();
    
    protected override void OnSetPanelDatas()
    {
        base.OnSetPanelDatas();
        
        bag = GetTargetPanelDatas<Bag>();
        inventory = bag.GetInventory(itemType);
        
        teamLists.Clear();
        for (int i = 0; i < inventory.Items.Count; i+= lowCount)
        {
            teamLists.Add(ItemList.Create(inventory.Items.Skip(i).Take(lowCount)));
        }

        scroller.Delegate ??= this;
        scroller.ReloadData();
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return teamLists.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return cellSize;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var cellObject = allocGameObject.AllocateObject();
        var cellView = cellObject.GetComponent<UITeamListCellView>();
        var teamList = teamLists[dataIndex];
        cellView.SetTeamList(teamList);
        cellView.SetMessageBus(TargetMessageBus);

        return cellView;
    }
}
