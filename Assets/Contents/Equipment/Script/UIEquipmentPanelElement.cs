using UnityEngine;

public class UIEquipmentPanelElement : PanelElement
{
    [SerializeField] UIItem uiPlayerItem;
    [SerializeField] UIItem uiWeaponItem;
    [SerializeField] UIItem uiArmorItem;
    [SerializeField] UIItem uiAccessoryItem;

    Item playerItem;
    Item weaponItem;
    Item armorItem;
    Item accessoryItem;
    
    public override void Initialize(Panel panel, IInitData initData = null)
    {
        base.Initialize(panel, initData);
        
        uiWeaponItem.SetClickEvent(ClickItem);
        uiArmorItem.SetClickEvent(ClickItem);
        uiAccessoryItem.SetClickEvent(ClickItem);
    }

    public override void Uninitialize()
    {
        uiWeaponItem.SetClickEvent(null);
        uiArmorItem.SetClickEvent(null);
        uiAccessoryItem.SetClickEvent(null);
        
        base.Uninitialize();
    }

    public void SetPlayerItem(Item item)
    {
        playerItem = item;
    }

    public void SetWeaponItem(Item item)
    {
        weaponItem = item;
    }
    
    public void SetArmorItem(Item item)
    {
        armorItem = item;
    }
    
    public void SetAccessoryItem(Item item)
    {
        accessoryItem = item;
    }

    void ClickItem(Item item)
    {
        if (!item.ItemData.CanEquip())
        {
            return;
        }

        var msg = new UIMsg.SelectEquipItemMsg()
        {
            Item = item
        };

        Panel.MessageBus.Publish(msg);
    }
}

public static partial class UIMsg
{
    public struct SelectEquipItemMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.UI;
        public Item Item;
    }
}



