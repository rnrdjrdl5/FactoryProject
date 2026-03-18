using System;
using System.Collections.Generic;
using System.Linq;

namespace Tables
{
    public static partial class EnumLogic
    {
        public static ItemType[] ItemTypes => (ItemType[])Enum.GetValues(typeof(ItemType));
        public static StatType[] StatTypes => (StatType[])Enum.GetValues(typeof(StatType));
        
        #region CacheEnum
        
        static string[] statNames;
        
        public static void CachingTable()
        {
            CachingStat();
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
        
        public static string GetStatName(StatType statType) => statNames[(int)statType];
        
        #endregion
    }
}