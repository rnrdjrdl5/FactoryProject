public class PlayerUIProcessor : Processor
{
    public void OpenInventory()
    {
        var panelAbility = Realm.GetAbility<PanelAbility>();
        var inventoryPopup = panelAbility.CreatePanel<InventoryPopup>(InventoryPopup.PrefabPath);
        var uiInventoryPanelElement = inventoryPopup.GetPanelElement<UIInventoryPanelElement>();
        uiInventoryPanelElement.SetItemType(Tables.ItemType.Player);
        
        inventoryPopup.SetTargetData(Entity, Entity.MessageBus);
    }
}