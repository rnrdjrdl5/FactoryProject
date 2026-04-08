using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamPopupProcessor : Processor
{
    TeamStorage teamStorage;
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
        teamPopup.MessageBus.Subscribe<UIMsg.SelectTeamInventoryItemMsg>(AddTeamFormationItem);
        teamPopup.MessageBus.Subscribe<UIMsg.ClickAddFormationMsg>(AddFormation);
        teamPopup.MessageBus.Subscribe<UIMsg.TeamFormationGoMsg>(GoFormation);
        teamPopup.MessageBus.Subscribe<UIMsg.PlaceTeamFormationMsg>(PlaceFormation);
    }

    public override void Uninitialize()
    {
        teamPopup.OnSetPanelDatasAction -= OnSetPanelDatasAction;
        teamPopup.MessageBus.Unsubscribe<UIMsg.SelectTeamFormationMsg>(SelectTeamFormation);
        teamPopup.MessageBus.Unsubscribe<UIMsg.RemoveTeamFormationItemMsg>(RemoveTeamFormationItem);
        teamPopup.MessageBus.Unsubscribe<UIMsg.RemoveTeamFormationMsg>(RemoveTeamFormation);
        teamPopup.MessageBus.Unsubscribe<UIMsg.SelectTeamInventoryItemMsg>(AddTeamFormationItem);
        teamPopup.MessageBus.Unsubscribe<UIMsg.ClickAddFormationMsg>(AddFormation);
        teamPopup.MessageBus.Unsubscribe<UIMsg.TeamFormationGoMsg>(GoFormation);
        teamPopup.MessageBus.Unsubscribe<UIMsg.PlaceTeamFormationMsg>(PlaceFormation);
        
        base.Uninitialize();
    }

    void OnSetPanelDatasAction()
    {
        teamStorage = teamPopup.GetTargetPanelDatas<TeamStorage>();
        
        var playerData = teamPopup.GetTargetPanelDatas<PlayerStorage>();
        teamInventory = playerData.PlayerInventory;
    }

    void SelectTeamFormation(UIMsg.SelectTeamFormationMsg msg)
    {
        if (teamStorage == null)
        {
            return;
        }
        
        teamStorage.SelectTeamFormation(msg.TeamFormationStorage);
    }

    void RemoveTeamFormationItem(UIMsg.RemoveTeamFormationItemMsg msg)
    {
        if (teamStorage == null)
        {
            return;
        }
        
        msg.TeamFormationStorage.RemovePlayer(msg.Item);
        teamInventory.Unequip(msg.Item);
        teamStorage.SelectTeamFormation(msg.TeamFormationStorage);
    }
    
    void RemoveTeamFormation(UIMsg.RemoveTeamFormationMsg msg)
    {
        if (teamStorage == null)
        {
            return;
        }
        
        teamStorage.TryRemoveTeamFormation(msg.TeamFormationStorage);
    }

    void AddTeamFormationItem(UIMsg.SelectTeamInventoryItemMsg msg)
    { 
        if (teamStorage == null || teamStorage.SelectedTeamFormation.Players.Contains(msg.Item))
        {
            return;
        }
        
        teamInventory.Equip(msg.Item);
        teamStorage.SelectedTeamFormation.TryAddPlayer(msg.Item);
    }

    void AddFormation(UIMsg.ClickAddFormationMsg msg)
    {
        if (teamStorage == null)
        {
            return;
        }
        
        teamStorage.AddTeamFormation();
    }

    void GoFormation(UIMsg.TeamFormationGoMsg msg)
    {
        var mainRealm = Realm.GetParent<MainRealm>();
        var processorAbility = mainRealm.GetAbility<ProcessorAbility>();
        var playerEntityProcessor = processorAbility.GetProcessor<MainRealmPlayerEntityProcessor>();
        playerEntityProcessor.CreateControlledHeroTeam(msg.TeamFormationStorage);
    }

    void PlaceFormation(UIMsg.PlaceTeamFormationMsg msg)
    {
        var mainRealm = Realm.GetParent<MainRealm>();
        var processorAbility = mainRealm.GetAbility<ProcessorAbility>();
        var playerEntityProcessor = processorAbility.GetProcessor<MainRealmPlayerEntityProcessor>();
        playerEntityProcessor.PlaceAIHeroTeam(msg.TeamFormationStorage);
    }
}
