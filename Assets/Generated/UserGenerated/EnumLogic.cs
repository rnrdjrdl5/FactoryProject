using System;
using System.Collections.Generic;
using System.Linq;

namespace Tables
{
    public static partial class EnumLogic
    {
        public static ItemType[] ItemTypes => (ItemType[])Enum.GetValues(typeof(ItemType));
        public static ItemSlotType[] ItemSlotTypes => (ItemSlotType[])Enum.GetValues(typeof(ItemSlotType));
        public static StatType[] StatTypes => (StatType[])Enum.GetValues(typeof(StatType));
        
        #region CacheEnum
        
        static string[] statNames;
        static string[] itemNames;
        
        public static void CachingTable()
        {
            CachingStat();
            CachingItem();
        }

        static void CachingStat()
        {
            var max = StatTypes.Max(v => (int)v);
            
            statNames = new string[max + 1];

            foreach (var v in StatTypes)
            {
                statNames[(int)v] = v.ToString();
            }
        }

        static void CachingItem()
        {
            var max = ItemTypes.Max(v => (int)v);
            
            itemNames = new string[max + 1];

            foreach (var v in ItemTypes)
            {
                itemNames[(int)v] = v.ToString();
            }
        }
        
        public static string GetStatName(StatType statType) => statNames[(int)statType];
        public static string GetItemName(ItemType itemType) => itemNames[(int)itemType];
        
        #endregion
    }
}