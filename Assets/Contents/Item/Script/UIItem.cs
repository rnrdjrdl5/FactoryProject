using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : UISetter
{
    [SerializeField] Image itemSprite;
    [SerializeField] Image gradeSprite;
    [SerializeField] TMP_Text descText;
    [SerializeField] TMP_Text amountText;
    [SerializeField] GameObject isEquipObject;
    
    IIconSprite iconData;
    IIconSprite gradeIconData;
    IDescription descriptionData;
    Action<Item> OnClickEvent;
    Item item;
    
    public void UpdateItemData(Item item)
    {
        this.item = item;
        
        UpdateItemData(item.ItemData, item.ItemData, item.ItemData, item.Amount, item.IsEquip);
    }
    
    public void UpdateItemData(IIconSprite iconData, IGradeType gradeData, IDescription descriptionData, int amount, bool isEquip)
    {
        var gradeIconData = gradeData.Grade as IIconSprite;
        
        SetImage(itemSprite, iconData.GetIconSprite());
        SetImage(gradeSprite, gradeIconData.GetIconSprite());
        SetText(descText, descriptionData.description);
        SetText(amountText, $"{amount}");
        SetObject(isEquipObject, isEquip);
    }

    public void SetClickEvent(Action<Item> onClickEvent)
    {
        OnClickEvent = onClickEvent;
    }

    public void OnClick()
    {
        OnClickEvent?.Invoke(item);
    }
}
