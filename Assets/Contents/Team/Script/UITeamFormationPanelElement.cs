using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

// NOTE : 추후 Refresh말고도 Redraw 추가
public class UITeamFormationPanelElement : PanelElement , IEnhancedScrollerDelegate
{
    [SerializeField] EnhancedScroller scroller;
    [SerializeField] float cellSize;
    [SerializeField] int lowCount;
    [SerializeField] AllocGameObject allocGameObject;

    IReadOnlyList<TeamFormation> teamFormationList;
    Tables.ItemType itemType = Tables.ItemType.Animal;
    TeamFormation selectedTeamFormation;
    Team team;
    
    
    protected override void OnSetPanelDatas()
    {
        base.OnSetPanelDatas();
        
        team = GetTargetPanelDatas<Team>();
        team.OnChanged += RefreshUI;

        RefreshUI();
    }

    protected override void OnUnsetPanelDatas()
    {
        if (team != null)
        {
            team.OnChanged -= RefreshUI;
        }

        base.OnUnsetPanelDatas();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        
        teamFormationList = team.TeamFormations;

        scroller.Delegate ??= this;
        scroller.ReloadData();
    }

    public void SetSelectedTeamFormation(TeamFormation teamFormation)
    {
        selectedTeamFormation = teamFormation;
        RefreshUI();
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return teamFormationList.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return cellSize;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var cellObject = allocGameObject.AllocateObject();
        var cellView = cellObject.GetComponent<UITeamFormationListCellView>();
        var teamFormation = teamFormationList[dataIndex];
        var isSelected = selectedTeamFormation != null && selectedTeamFormation == teamFormation;
        cellView.Initialize(team, teamFormation, this, isSelected);

        return cellView;
    }

    public void OnClickAddFormation()
    {
        var msg = new ClickAddFormationMsg();
        Panel.MessageBus.Publish(msg);
    }
}

public class ClickAddFormationMsg
{
    
}