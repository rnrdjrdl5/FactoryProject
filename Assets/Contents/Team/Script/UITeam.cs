using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITeam : UISetter 
{
    [SerializeField] Image itemSprite;
    [SerializeField] Image gradeSprite;
    [SerializeField] TMP_Text descText;
    
    IIconAtlasSprite iconAtlasData;
    IIconAtlasSprite gradeIconAtlasData;
    IDescription descriptionData;

    public void UpdateItemData(Item item)
    {
        UpdateItemData(item.ItemData, item.ItemData, item.ItemData, item.Amount);
    }
    
    public void UpdateItemData(IIconSprite iconAtlasData, IGradeType gradeData, IDescription descriptionData, int amount)
    {
        var gradeIconData = gradeData.Grade as IIconAtlasSprite;
        
        SetImage(itemSprite, iconAtlasData.GetIconSprite());
        SetImage(gradeSprite, gradeIconData.GetIconSprite());
        SetText(descText, descriptionData.description);
    }
}
