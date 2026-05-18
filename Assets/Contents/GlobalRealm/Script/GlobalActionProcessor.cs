using UnityEngine;

public class GlobalActionProcessor : Processor
{
    MainStorage mainStorage;
    TeamStorage teamStorage;
    
    public override void Ready()
    {
        base.Ready();

        mainStorage = FactoryEntry.MainStorage;
        teamStorage = mainStorage.GetEntityData<TeamStorage>();
    }

    public void OpenTeam()
    {
        OpenTeam(default);
    }

    public void OpenTeam(ContentInputContext<TeamInputType> contentInputContext)
    {
        var panelAbility = Realm.GetAbility<PanelAbility>();
        var teamPopup = panelAbility.CreatePanel<TeamPopup>(TeamPopup.PrefabPath);
        teamPopup.SetTargetData(mainStorage, mainStorage.MessageBus);
    }

    public void OpenEquipment()
    {
        OpenEquipment(default);
    }

    public void OpenEquipment(ContentInputContext<EquipmentInputType> contentInputContext)
    {
        var panelAbility = Realm.GetAbility<PanelAbility>();
        var equipmentPopup = panelAbility.CreatePanel<EquipmentPopup>(EquipmentPopup.PrefabPath);
        equipmentPopup.SetTargetData(mainStorage, mainStorage.MessageBus);
        
        var uiInventoryPanelElement = equipmentPopup.GetPanelElement<UIInventoryPanelElement>();
        var bag = mainStorage.GetEntityData<Bag>();
        uiInventoryPanelElement.SetTargetPanelDatas(new []{bag});
    }

    public void OpenInventory()
    {
        OpenInventory(default);
    }

    public void OpenInventory(ContentInputContext<InventoryInputType> contentInputContext)
    {
        var panelAbility = Realm.GetAbility<PanelAbility>();
        var inventoryPopup = panelAbility.CreatePanel<InventoryPopup>(InventoryPopup.PrefabPath);
        var bag = mainStorage.GetEntityData<Bag>();
        inventoryPopup.SetExternalMessageBus(mainStorage.MessageBus);
        inventoryPopup.SetTargetPanelDatas(new []{bag});
    }
}
