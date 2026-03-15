using System.Collections.Generic;
using UnityEngine;

public class TeamPopup : Panel, IPanelOrderType
{
    public static string PrefabPath = $"Team/{nameof(TeamPopup)}";
    public PanelOrderType PanelOrderType { get; set; } = PanelOrderType.Popup;
}
