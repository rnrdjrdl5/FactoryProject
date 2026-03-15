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
    Team team;
    
    protected override void OnSetPanelDatas()
    {
        base.OnSetPanelDatas();
        
        team = GetTargetPanelDatas<Team>();
        if (team?.MessageBus != null)
        {
            team.MessageBus.Subscribe<TeamFormationAddedMsg>(OnTeamFormationAdded);
            team.MessageBus.Subscribe<TeamFormationRemovedMsg>(OnTeamFormationRemoved);
            team.MessageBus.Subscribe<TeamSelectedFormationChangedMsg>(OnTeamSelectedFormationChanged);
        }

        RefreshUI();
    }

    protected override void OnUnsetPanelDatas()
    {
        if (team != null)
        {
            if (team.MessageBus != null)
            {
                team.MessageBus.Unsubscribe<TeamFormationAddedMsg>(OnTeamFormationAdded);
                team.MessageBus.Unsubscribe<TeamFormationRemovedMsg>(OnTeamFormationRemoved);
                team.MessageBus.Unsubscribe<TeamSelectedFormationChangedMsg>(OnTeamSelectedFormationChanged);
            }
        }

        base.OnUnsetPanelDatas();
    }

    void OnTeamFormationAdded(TeamFormationAddedMsg msg)
    {
        if (msg.Team != team)
            return;

        RefreshUI();
    }

    void OnTeamFormationRemoved(TeamFormationRemovedMsg msg)
    {
        if (msg.Team != team)
            return;

        RefreshUI();
    }

    void OnTeamSelectedFormationChanged(TeamSelectedFormationChangedMsg msg)
    {
        if (msg.Team != team)
            return;

        RefreshUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        
        teamFormationList = team.TeamFormations;

        scroller.Delegate ??= this;
        scroller.ReloadData();
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
        var isSelected = team.SelectedTeamFormation != null && team.SelectedTeamFormation == teamFormation;
        cellView.Initialize(team, teamFormation, this, isSelected);

        return cellView;
    }

    public void OnClickAddFormation()
    {
        var msg = new ClickAddFormationMsg();
        Panel.MessageBus.Publish(msg);
    }
}

public struct ClickAddFormationMsg
{
    
}
