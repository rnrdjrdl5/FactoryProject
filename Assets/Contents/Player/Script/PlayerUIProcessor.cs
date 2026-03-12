public class PlayerUIProcessor : Processor
{
    public void OpenInpventory()
    {
        var panelAbility = Realm.GetAbility<PanelAbility>();
        var inventoryPopup = panelAbility.CreatePanel<InventoryPopup>(InventoryPopup.PrefabPath);
        var uiInventoryPanelElement = inventoryPopup.GetPanelElement<UIInventoryPanelElement>();
        uiInventoryPanelElement.SetItemType(Tables.ItemType.Animal);
        
        inventoryPopup.SetTargetPanelDatas(Entity.ToData());
    }
}