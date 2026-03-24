using UnityEngine;

public class GlobalProcessor : Processor
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
        var equipmentPopup = panelAbility.CreatePanel<EquipmentPopup>(EquipmentPopup.PrefabPath);
        equipmentPopup.SetTargetData(mainStorage, mainStorage.MessageBus);
    }

    public void OpenInventory()
    {
        var panelAbility = Realm.GetAbility<PanelAbility>();
        var inventoryPopup = panelAbility.CreatePanel<InventoryPopup>(InventoryPopup.PrefabPath);
        inventoryPopup.SetTargetData(mainStorage, mainStorage.MessageBus);
    }
}
