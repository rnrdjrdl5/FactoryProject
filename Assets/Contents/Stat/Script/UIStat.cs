using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStat : UISetter
{
    [SerializeField] Image iconSprite;
    [SerializeField] TMP_Text statValueText;
    
    Tables.StatType statType;
    
    public void UpdateStat(Tables.StatType statType, int statValue)
    {
        this.statType = statType;

        var iconData = Tables.EnumLogic.GetMetaData(statType) as IIconAtlasSprite;
        iconSprite.sprite = iconData.GetIconSprite();
        statValueText.text = $"{statValue}";
    }
}
