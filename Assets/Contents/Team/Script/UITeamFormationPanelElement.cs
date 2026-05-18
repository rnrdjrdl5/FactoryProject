using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

// NOTE: Add Redraw support later if Refresh is not enough.
public class UITeamFormationPanelElement : PanelElement , IEnhancedScrollerDelegate
{
    [SerializeField] EnhancedScroller scroller;
    [SerializeField] float cellSize;
    [SerializeField] int lowCount;
    [SerializeField] AllocGameObject allocGameObject;

    IReadOnlyList<TeamFormationStorage> teamFormationList;
    Tables.ItemType itemType = Tables.ItemType.Player;
    TeamStorage teamStorage;
    
    protected override void OnSetPanelDatas()
    {
        base.OnSetPanelDatas();
        
        teamStorage = GetTargetPanelDatas<TeamStorage>();
        if (teamStorage?.MessageBus != null)
        {
            teamStorage.MessageBus.Subscribe<EntityDataMsg.TeamFormationAddedMsg>(OnTeamFormationAdded);
            teamStorage.MessageBus.Subscribe<EntityDataMsg.TeamFormationRemovedMsg>(OnTeamFormationRemoved);
            teamStorage.MessageBus.Subscribe<EntityDataMsg.TeamFormationChangedMsg>(OnTeamFormationChanged);
            teamStorage.MessageBus.Subscribe<EntityDataMsg.TeamSelectedFormationChangedMsg>(OnTeamSelectedFormationChanged);
        }

        RefreshUI();
    }

    protected override void OnUnsetPanelDatas()
    {
        if (teamStorage != null)
        {
            if (teamStorage.MessageBus != null)
            {
                teamStorage.MessageBus.Unsubscribe<EntityDataMsg.TeamFormationAddedMsg>(OnTeamFormationAdded);
                teamStorage.MessageBus.Unsubscribe<EntityDataMsg.TeamFormationRemovedMsg>(OnTeamFormationRemoved);
                teamStorage.MessageBus.Unsubscribe<EntityDataMsg.TeamFormationChangedMsg>(OnTeamFormationChanged);
                teamStorage.MessageBus.Unsubscribe<EntityDataMsg.TeamSelectedFormationChangedMsg>(OnTeamSelectedFormationChanged);
            }
        }

        base.OnUnsetPanelDatas();
    }

    void OnTeamFormationAdded(EntityDataMsg.TeamFormationAddedMsg msg)
    {
        if (msg.TeamStorage != teamStorage)
            return;

        RefreshUI();
    }

    void OnTeamFormationRemoved(EntityDataMsg.TeamFormationRemovedMsg msg)
    {
        if (msg.TeamStorage != teamStorage)
            return;

        RefreshUI();
    }

    void OnTeamFormationChanged(EntityDataMsg.TeamFormationChangedMsg msg)
    {
        if (teamStorage == null || msg.Formation == null || !teamStorage.TeamFormations.Contains(msg.Formation))
            return;

        RefreshUI();
    }

    void OnTeamSelectedFormationChanged(EntityDataMsg.TeamSelectedFormationChangedMsg msg)
    {
        if (msg.TeamStorage != teamStorage)
            return;

        RefreshUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        
        teamFormationList = teamStorage.TeamFormations;

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
        var isSelected = teamStorage.SelectedTeamFormation != null && teamStorage.SelectedTeamFormation == teamFormation;
        cellView.Initialize(teamStorage, teamFormation, this, isSelected);

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
