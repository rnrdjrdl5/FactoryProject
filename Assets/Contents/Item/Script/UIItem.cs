using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : UISetter
{
    [SerializeField] Image itemSprite;
    [SerializeField] Image gradeSprite;
    [SerializeField] TMP_Text descText;
    
    IIconSprite iconData;
    IIconSprite gradeIconData;
    IDescription descriptionData;

    public void UpdateItemData(Item item)
    {
        UpdateItemData(item.ItemData, item.ItemData, item.ItemData);
    }
    
    public void UpdateItemData(IIconSprite iconData, IGradeType gradeData, IDescription descriptionData)
    {
        this.iconData = iconData;
        gradeIconData = gradeData.Grade;
        this.descriptionData = descriptionData;
        UpdateUIItem();
    }

    void UpdateUIItem()
    {
        SetImage(itemSprite, iconData.GetIconSprite());
        SetImage(gradeSprite, gradeIconData.GetIconSprite());
        SetText(descText, descriptionData.description);
    }
}
