using UnityEngine;

public class GlobalProcessor : Processor
{
    MessageBus messageBus;
    MainStorage mainStorage;
    Team team;
    
    public override void Ready()
    {
        base.Ready();

        mainStorage = FactoryEntry.MainStorage;
        team = mainStorage.GetEntityData<Team>();
        
        messageBus = Entity.GetEntityData<MessageBus>();
        messageBus.Subscribe<AddFormationMessage>(AddTeam);
    }

    public override void Uninitialize()
    {
        messageBus.Unsubscribe<AddFormationMessage>(AddTeam);
        
        base.Uninitialize();
    }

    public void OpenTeam()
    {
        var panelAbility = Realm.GetAbility<PanelAbility>();
        var teamPopup = panelAbility.CreatePanel<TeamPopup>(TeamPopup.PrefabPath);
        var uiInventoryPanelElement = teamPopup.GetPanelElement<UIInventoryPanelElement>();
        uiInventoryPanelElement.SetItemType(Tables.ItemType.Animal);
        
        teamPopup.SetTargetData(mainStorage, mainStorage.MessageBus);
    }

    void AddTeam(AddFormationMessage msg)
    {
        team.AddTeamFormation();
    }
}
