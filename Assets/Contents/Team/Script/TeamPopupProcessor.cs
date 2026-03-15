using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamPopupProcessor : Processor
{
    const string FormationDefaultName = "Formation";
    
    TeamPopup teamPopup;
    UITeamFormationPanelElement uiTeamFormationPanelElement;
    UIInventoryPanelElement uiInventoryPanelElement;
    TeamFormation selectedTeamFormation;

    public override void Ready()
    {
        base.Ready();
        
        teamPopup = Entity as TeamPopup;
        uiTeamFormationPanelElement = teamPopup.GetPanelElement<UITeamFormationPanelElement>();
        uiInventoryPanelElement = teamPopup.GetPanelElement<UIInventoryPanelElement>();
        
        teamPopup.MessageBus.Subscribe<SelectTeamFormationMsg>(SelectTeamFormation);
        teamPopup.MessageBus.Subscribe<RemoveTeamFormationItemMsg>(RemoveTeamFormationItem);
        teamPopup.MessageBus.Subscribe<RemoveTeamFormationMsg>(RemoveTeamFormation);
        teamPopup.MessageBus.Subscribe<ClickInventoryItemMsg>(AddTeamFormationItem);
        teamPopup.MessageBus.Subscribe<ClickAddFormationMsg>(AddFormation);

        UpdateEquipItemKeys();
        
    }

    public override void Uninitialize()
    {
        teamPopup.MessageBus.Unsubscribe<SelectTeamFormationMsg>(SelectTeamFormation);
        teamPopup.MessageBus.Unsubscribe<RemoveTeamFormationItemMsg>(RemoveTeamFormationItem);
        teamPopup.MessageBus.Unsubscribe<RemoveTeamFormationMsg>(RemoveTeamFormation);
        teamPopup.MessageBus.Unsubscribe<ClickInventoryItemMsg>(AddTeamFormationItem);
        
        base.Uninitialize();
    }

    void SelectTeamFormation(SelectTeamFormationMsg msg)
    {
        selectedTeamFormation = msg.TeamFormation;
        uiTeamFormationPanelElement.SetSelectedTeamFormation(msg.TeamFormation);
    }

    void RemoveTeamFormationItem(RemoveTeamFormationItemMsg msg)
    {
        msg.TeamFormation.RemovePlayer(msg.Item);
        
        selectedTeamFormation = msg.TeamFormation;
        uiTeamFormationPanelElement.SetSelectedTeamFormation(msg.TeamFormation);

        UpdateEquipItemKeys();
    }
    
    void RemoveTeamFormation(RemoveTeamFormationMsg msg)
    {
        msg.Team.TryRemoveTeamFormation(msg.TeamFormation);

        UpdateEquipItemKeys();
    }

    void AddTeamFormationItem(ClickInventoryItemMsg msg)
    {
        if (selectedTeamFormation.Players.Contains(msg.Item))
        {
            return;
        }
        
        selectedTeamFormation.TryAddPlayer(msg.Item);

        UpdateEquipItemKeys();
    }

    void AddFormation(ClickAddFormationMsg msg)
    {
        var team = teamPopup.GetTargetPanelDatas<Team>();
        var nextCount = team.TeamFormations.Count + 1;
        
        team.AddTeamFormation($"{FormationDefaultName} {nextCount}");
    }

    void UpdateEquipItemKeys()
    {
        if (uiInventoryPanelElement == null)
        {
            return;
        }
        
        var team = teamPopup.GetTargetPanelDatas<Team>();
        if (team == null)
        {
            return;
        }

        var bag = teamPopup.GetTargetPanelDatas<Bag>();
        var inventory = bag.GetInventory(Tables.ItemType.Animal);
        var equipItemKeys = team.GetEquipItemKeys(inventory.Items);
        
        SetEquipItemKeys(equipItemKeys);
    }
    
    public void SetEquipItemKeys(IEnumerable<Item> items)
    {
        uiInventoryPanelElement.SetEquipItemKeys(items);
    }
}
