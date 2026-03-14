using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class UITeamFormationPanelElement : PanelElement , IEnhancedScrollerDelegate
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

        RefreshUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        
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

        return cellView;
    }

    public void OnClickAddFormation()
    {
        MessageBus.Publish(new AddFormationMessage());
    }
}

public class AddFormationMessage
{

}
