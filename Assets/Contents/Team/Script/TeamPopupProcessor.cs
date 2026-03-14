using UnityEngine;

public class TeamPopupProcessor : Processor
{
    TeamPopup teamPopup;
    UITeamFormationPanelElement uiTeamFormationPanelElement;

    public override void Ready()
    {
        base.Ready();
        
        teamPopup = Entity as TeamPopup;
        uiTeamFormationPanelElement = teamPopup.GetPanelElement<UITeamFormationPanelElement>();
        
        Entity.MessageBus.Subscribe<SelectTeamFormationMsg>(SelectTeamFormation);
        Entity.MessageBus.Subscribe<RemoveTeamFormationItemMsg>(RemoveTeamFormationItem);
        Entity.MessageBus.Subscribe<RemoveTeamFormationMsg>(RemoveTeamFormation);
    }

    public override void Uninitialize()
    {
        Entity.MessageBus.Unsubscribe<SelectTeamFormationMsg>(SelectTeamFormation);
        Entity.MessageBus.Unsubscribe<RemoveTeamFormationItemMsg>(RemoveTeamFormationItem);
        Entity.MessageBus.Unsubscribe<RemoveTeamFormationMsg>(RemoveTeamFormation);
        
        base.Uninitialize();
    }

    void SelectTeamFormation(SelectTeamFormationMsg msg)
    {
        uiTeamFormationPanelElement.SetSelectedTeamFormation(msg.TeamFormation);
    }

    void RemoveTeamFormationItem(RemoveTeamFormationItemMsg msg)
    {
        msg.TeamFormation.RemovePlayer(msg.Item);
        uiTeamFormationPanelElement.SetSelectedTeamFormation(msg.TeamFormation);
    }
    
    void RemoveTeamFormation(RemoveTeamFormationMsg msg)
    {
        msg.Team.TryRemoveTeamFormation(msg.TeamFormation);
    }
}
