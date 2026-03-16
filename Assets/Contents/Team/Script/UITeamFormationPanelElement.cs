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
    Tables.ItemType itemType = Tables.ItemType.Player;
    Team team;
    
    protected override void OnSetPanelDatas()
    {
        base.OnSetPanelDatas();
        
        team = GetTargetPanelDatas<Team>();
        if (team?.MessageBus != null)
        {
            team.MessageBus.Subscribe<EntityDataMsg.TeamFormationAddedMsg>(OnTeamFormationAdded);
            team.MessageBus.Subscribe<EntityDataMsg.TeamFormationRemovedMsg>(OnTeamFormationRemoved);
            team.MessageBus.Subscribe<EntityDataMsg.TeamSelectedFormationChangedMsg>(OnTeamSelectedFormationChanged);
        }

        RefreshUI();
    }

    protected override void OnUnsetPanelDatas()
    {
        if (team != null)
        {
            if (team.MessageBus != null)
            {
                team.MessageBus.Unsubscribe<EntityDataMsg.TeamFormationAddedMsg>(OnTeamFormationAdded);
                team.MessageBus.Unsubscribe<EntityDataMsg.TeamFormationRemovedMsg>(OnTeamFormationRemoved);
                team.MessageBus.Unsubscribe<EntityDataMsg.TeamSelectedFormationChangedMsg>(OnTeamSelectedFormationChanged);
            }
        }

        base.OnUnsetPanelDatas();
    }

    void OnTeamFormationAdded(EntityDataMsg.TeamFormationAddedMsg msg)
    {
        if (msg.Team != team)
            return;

        RefreshUI();
    }

    void OnTeamFormationRemoved(EntityDataMsg.TeamFormationRemovedMsg msg)
    {
        if (msg.Team != team)
            return;

        RefreshUI();
    }

    void OnTeamSelectedFormationChanged(EntityDataMsg.TeamSelectedFormationChangedMsg msg)
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
        var msg = new UIMsg.ClickAddFormationMsg();
        Panel.MessageBus.Publish(msg);
    }
}

public static partial class UIMsg
{
    public struct ClickAddFormationMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.UI;
    }
}
