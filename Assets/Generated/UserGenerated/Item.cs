using System;
using Unity.VisualScripting;

namespace Tables
{
    public partial class Item : IStats , IIconSprite, IGradeType , IDescription
    {
        public static bool TryCanEquip(string itemKey, out bool result)
        {
            result = false;
            
            var itemData = Item.Get(itemKey);
            if (itemData == null)
            {
                return false;
            }

            result = CanEquip(itemData);
            return true;
        }
        
        public static bool CanEquip(Item item)
        {
            return item.itemType is ItemType.Accessory or ItemType.Armor or ItemType.Weapon;
        }
    }
}
