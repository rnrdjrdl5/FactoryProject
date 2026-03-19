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
        
        teamPopup.SetTargetData(mainStorage, mainStorage.MessageBus);
    }

    public void OpenEquipment()
    {
        var panelAbility = Realm.GetAbility<PanelAbility>();
        var teamPopup = panelAbility.CreatePanel<EquipmentPopup>(EquipmentPopup.PrefabPath);
        
        teamPopup.SetTargetData(mainStorage, mainStorage.MessageBus);
    }
}
