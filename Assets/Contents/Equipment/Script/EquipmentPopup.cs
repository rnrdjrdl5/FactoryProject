using UnityEngine;

public class EquipmentPopup : Panel , IPanelOrderType
{
    public static string PrefabPath = $"Equipment/{nameof(EquipmentPopup)}";
    public PanelOrderType PanelOrderType { get; set; } = PanelOrderType.Popup; 
}
