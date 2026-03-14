using UnityEngine;

public class GlobalTeamProcessor : Processor
{
    MainStorage mainStorage;
    Team team;
    
    public override void Ready()
    {
        base.Ready();

        mainStorage = FactoryEntry.MainStorage;
        team = mainStorage.GetEntityData<Team>();
    }

    public void OpenTeam()
    {
        var panelAbility = Realm.GetAbility<PanelAbility>();
        var teamPopup = panelAbility.CreatePanel<TeamPopup>(TeamPopup.PrefabPath);
        var uiInventoryPanelElement = teamPopup.GetPanelElement<UIInventoryPanelElement>();
        uiInventoryPanelElement.SetItemType(Tables.ItemType.Animal);
        
        teamPopup.SetTargetData(mainStorage, mainStorage.MessageBus);
    }
}
