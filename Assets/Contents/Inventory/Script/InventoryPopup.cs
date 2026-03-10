using UnityEngine;

public class InventoryPopup : Panel, IPanelOrderType
{
    public static string PrefabPath = $"Inventory/{nameof(InventoryPopup)}";
    public PanelOrderType PanelOrderType { get; set; } = PanelOrderType.Popup;
}
