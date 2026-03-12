using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : UISetter
{
    [SerializeField] Image itemSprite;
    [SerializeField] Image gradeSprite;
    [SerializeField] TMP_Text descText;
    [SerializeField] TMP_Text amountText;
    
    IIconSprite iconData;
    IIconSprite gradeIconData;
    IDescription descriptionData;

    public void UpdateItemData(Item item)
    {
        UpdateItemData(item.ItemData, item.ItemData, item.ItemData, item.Amount);
    }
    
    public void UpdateItemData(IIconSprite iconData, IGradeType gradeData, IDescription descriptionData, int amount)
    {
        var gradeIconData = gradeData.Grade as IIconSprite;
        
        SetImage(itemSprite, iconData.GetIconSprite());
        SetImage(gradeSprite, gradeIconData.GetIconSprite());
        SetText(descText, descriptionData.description);
        SetText(amountText, $"{amount}");
    }
}
