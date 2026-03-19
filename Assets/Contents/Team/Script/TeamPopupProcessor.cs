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
        teamPopup.OnSetPanelDatasAction += OnSetPanelDatasAction;
    }

    public override void Ready()
    {
        base.Ready();
        
        teamPopup.MessageBus.Subscribe<UIMsg.SelectTeamFormationMsg>(SelectTeamFormation);
        teamPopup.MessageBus.Subscribe<UIMsg.RemoveTeamFormationItemMsg>(RemoveTeamFormationItem);
        teamPopup.MessageBus.Subscribe<UIMsg.RemoveTeamFormationMsg>(RemoveTeamFormation);
        teamPopup.MessageBus.Subscribe<UIMsg.SelectInventoryItemMsg>(AddTeamFormationItem);
        teamPopup.MessageBus.Subscribe<UIMsg.ClickAddFormationMsg>(AddFormation);
        teamPopup.MessageBus.Subscribe<UIMsg.TeamFormationGoMsg>(GoFormation);
    }

    public override void Uninitialize()
    {
        teamPopup.OnSetPanelDatasAction -= OnSetPanelDatasAction;
        teamPopup.MessageBus.Unsubscribe<UIMsg.SelectTeamFormationMsg>(SelectTeamFormation);
        teamPopup.MessageBus.Unsubscribe<UIMsg.RemoveTeamFormationItemMsg>(RemoveTeamFormationItem);
        teamPopup.MessageBus.Unsubscribe<UIMsg.RemoveTeamFormationMsg>(RemoveTeamFormation);
        teamPopup.MessageBus.Unsubscribe<UIMsg.SelectInventoryItemMsg>(AddTeamFormationItem);
        
        base.Uninitialize();
    }

    void OnSetPanelDatasAction()
    {
        team = teamPopup.GetTargetPanelDatas<Team>();
        
        var bag = teamPopup.GetTargetPanelDatas<Bag>();
        teamInventory = bag?.GetInventory(Tables.ItemType.Player);
    }

    void SelectTeamFormation(UIMsg.SelectTeamFormationMsg msg)
    {
        if (team == null)
        {
            return;
        }
        
        team.SelectTeamFormation(msg.TeamFormation);
    }

    void RemoveTeamFormationItem(UIMsg.RemoveTeamFormationItemMsg msg)
    {
        if (team == null)
        {
            return;
        }
        
        msg.TeamFormation.RemovePlayer(msg.Item);
        teamInventory.Unequip(msg.Item);
        team.SelectTeamFormation(msg.TeamFormation);
    }
    
    void RemoveTeamFormation(UIMsg.RemoveTeamFormationMsg msg)
    {
        if (team == null)
        {
            return;
        }
        
        team.TryRemoveTeamFormation(msg.TeamFormation);
    }

    void AddTeamFormationItem(UIMsg.SelectInventoryItemMsg msg)
    { 
        if (team == null || team.SelectedTeamFormation.Players.Contains(msg.Item))
        {
            return;
        }
        
        teamInventory.Equip(msg.Item);
        team.SelectedTeamFormation.TryAddPlayer(msg.Item);
    }

    void AddFormation(UIMsg.ClickAddFormationMsg msg)
    {
        if (team == null)
        {
            return;
        }
        
        team.AddTeamFormation();
    }

    void GoFormation(UIMsg.TeamFormationGoMsg msg)
    {
        var mainRealm = Realm.GetParent<MainRealm>();
        var processorAbility = mainRealm.GetAbility<ProcessorAbility>();
        var mainRealmTeamProcessor = processorAbility.GetProcessor<MainRealmTeamProcessor>();
        mainRealmTeamProcessor.CreatePlayerByTeamFormation(msg.TeamFormation);
    }
}
