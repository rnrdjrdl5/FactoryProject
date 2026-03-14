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
        messageBus = Entity.GetEntityData<MessageBus>();
        messageBus.Subscribe<AddFormationMessage>(AddTeam);
        team = Entity.GetEntityData<Team>();
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
        
        teamPopup.SetTargetPanelDatas(mainStorage.ToData());
    }

    void AddTeam(AddFormationMessage msg)
    {
        team.AddTeamFormation();
    }
}
