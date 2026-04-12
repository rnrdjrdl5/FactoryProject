using System.Collections.Generic;
using System.Linq;

namespace Tables
{
    public static partial class EnumLogic
    {
        static Dictionary<StatType, StatInfo> slotMetaData = new()
        {
            {
                StatType.Str, new()
                {
                    iconAtlasPath = "Stat/icon_stat",
                    iconSpritePath = "icon_stat_str"
                }
                
            },
            
            {
                StatType.Dex, new()
                {
                    iconAtlasPath = "Stat/icon_stat",
                    iconSpritePath = "icon_stat_dex"
                }
            },
            
            {
                StatType.Int, new()
                {
                    iconAtlasPath = "Stat/icon_stat",
                    iconSpritePath = "icon_stat_int"
                }
            },
            
            {
                StatType.Physical, new()
                {
                    iconAtlasPath = "Stat/icon_stat",
                    iconSpritePath = "icon_stat_physical"
                }
            },
            
            {
                StatType.Magical, new()
                {
                    iconAtlasPath = "Stat/icon_stat",
                    iconSpritePath = "icon_stat_magical"
                }
            },
        };
        
        public static StatInfo GetMetaData(StatType type) => slotMetaData[type];
    }

    public class StatInfo : IIconAtlasSprite
    {
        public string iconAtlasPath { get; set; }
        public string iconSpritePath { get; set; }
    }
}