using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamPopupProcessor : Processor
{
    Team team;
    Inventory teamInventory;
    TeamPopup teamPopup;
    
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        teamPopup = Entity as TeamPopup;
        teamPopup.OnSetPanelDatas += OnSetPanelDatas;
    }

    public override void Ready()
    {
        base.Ready();
        
        teamPopup.MessageBus.Subscribe<SelectTeamFormationMsg>(SelectTeamFormation);
        teamPopup.MessageBus.Subscribe<RemoveTeamFormationItemMsg>(RemoveTeamFormationItem);
        teamPopup.MessageBus.Subscribe<RemoveTeamFormationMsg>(RemoveTeamFormation);
        teamPopup.MessageBus.Subscribe<SelectInventoryItemMsg>(AddTeamFormationItem);
        teamPopup.MessageBus.Subscribe<ClickAddFormationMsg>(AddFormation);
        
    }

    public override void Uninitialize()
    {
        teamPopup.OnSetPanelDatas -= OnSetPanelDatas;
        teamPopup.MessageBus.Unsubscribe<SelectTeamFormationMsg>(SelectTeamFormation);
        teamPopup.MessageBus.Unsubscribe<RemoveTeamFormationItemMsg>(RemoveTeamFormationItem);
        teamPopup.MessageBus.Unsubscribe<RemoveTeamFormationMsg>(RemoveTeamFormation);
        teamPopup.MessageBus.Unsubscribe<SelectInventoryItemMsg>(AddTeamFormationItem);
        
        base.Uninitialize();
    }

    void OnSetPanelDatas()
    {
        team = teamPopup.GetTargetPanelDatas<Team>();
        
        var bag = teamPopup.GetTargetPanelDatas<Bag>();
        teamInventory = bag?.GetInventory(Tables.ItemType.Animal);
    }

    void SelectTeamFormation(SelectTeamFormationMsg msg)
    {
        if (team == null)
        {
            return;
        }
        
        team.SelectTeamFormation(msg.TeamFormation);
    }

    void RemoveTeamFormationItem(RemoveTeamFormationItemMsg msg)
    {
        if (team == null)
        {
            return;
        }
        
        msg.TeamFormation.RemovePlayer(msg.Item);
        teamInventory.Unequip(msg.Item);
        team.SelectTeamFormation(msg.TeamFormation);
    }
    
    void RemoveTeamFormation(RemoveTeamFormationMsg msg)
    {
        if (team == null)
        {
            return;
        }
        
        team.TryRemoveTeamFormation(msg.TeamFormation);
    }

    void AddTeamFormationItem(SelectInventoryItemMsg msg)
    { 
        if (team == null || team.SelectedTeamFormation.Players.Contains(msg.Item))
        {
            return;
        }
        
        teamInventory.Equip(msg.Item);
        team.SelectedTeamFormation.TryAddPlayer(msg.Item);
    }

    void AddFormation(ClickAddFormationMsg msg)
    {
        if (team == null)
        {
            return;
        }
        
        team.AddTeamFormation();
    }
}
