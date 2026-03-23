public class PlayerUIProcessor : Processor
{
    public void OpenInventory()
    {
        var playerData = Entity.GetEntityData<PlayerData>();
        var bag = playerData.Bag;
        var panelAbility = Realm.GetAbility<PanelAbility>();
        var inventoryPopup = panelAbility.CreatePanel<InventoryPopup>(InventoryPopup.PrefabPath);
        var uiInventoryPanelElement = inventoryPopup.GetPanelElement<UIInventoryPanelElement>();
        uiInventoryPanelElement.SetData(bag, Tables.ItemType.Player);
        
        inventoryPopup.SetTargetData(Entity, Entity.MessageBus);
    }
}