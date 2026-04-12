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
    
    IIconAtlasSprite iconAtlasData;
    IIconAtlasSprite gradeIconAtlasData;
    IDescription descriptionData;
    Action<Item> OnClickEvent;
    Item item;
    
    public void UpdateItemData(Item item)
    {
        this.item = item;
        
        UpdateItemData(item.ItemData, item.ItemData, item.ItemData, item.Amount, item.IsEquip);
    }

    public void UpdateEmptyItemData()
    {
        var gradeAtlasSprite = Tables.Grade.Get(Tables.TablesKey.Grade_Common) as IIconAtlasSprite;
        SetImage(gradeSprite, gradeAtlasSprite.GetIconSprite());
        
        SetActiveObjects(false);
        SetObject(isEquipObject, false);
    }
    
    public void UpdateItemData(IIconSprite iconData, IGradeType gradeData, IDescription descriptionData, int amount, bool isEquip)
    {
        SetActiveObjects(true);
        
        var gradeIconData = gradeData.Grade as IIconAtlasSprite;
        SetImage(itemSprite, iconData.GetIconSprite());
        SetImage(gradeSprite, gradeIconData.GetIconSprite());
        SetText(descText, descriptionData.description);
        SetText(amountText, $"{amount}");
        SetObject(isEquipObject, isEquip);
    }

    void SetActiveObjects(bool isActive)
    {
        SetObject(itemSprite?.gameObject, isActive);
        SetObject(descText?.gameObject, isActive);
        SetObject(amountText?.gameObject, isActive);
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
