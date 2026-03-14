using UnityEngine;

public class TeamPopup : Panel, IPanelOrderType
{
    public static string PrefabPath = $"Team/{nameof(TeamPopup)}";
    public PanelOrderType PanelOrderType { get; set; } = PanelOrderType.Popup;

    UIInventoryPanelElement uiInventoryPanelElement;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);
        
        uiInventoryPanelElement = GetPanelElement<UIInventoryPanelElement>();
    }
}
