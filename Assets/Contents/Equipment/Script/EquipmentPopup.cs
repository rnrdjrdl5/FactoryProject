using UnityEngine;
public class EquipmentPopup : Panel , IPanelOrderType
{
    public static string PrefabPath = $"Equipment/{nameof(EquipmentPopup)}";
    public EquipmentPopupView View => view;
    public PanelOrderType PanelOrderType { get; set; } = PanelOrderType.Popup;

    [SerializeField] EquipmentPopupView view;

    protected override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);
    }

    public void OnClickClose()
    {
        Close();
    }
}
