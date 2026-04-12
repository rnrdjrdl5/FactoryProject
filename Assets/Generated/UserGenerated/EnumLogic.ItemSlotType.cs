using System.Collections.Generic;

namespace Tables
{
    public static partial class EnumLogic
    {
        static Dictionary<ItemSlotType, ItemType[]> itemSlotMappingData = new()
        {
            {
                ItemSlotType.Gold, System.Array.Empty<ItemType>()
            },
            
            {
                ItemSlotType.Player, new[]
                {
                    ItemType.Player
                }
            },
            
            {
                ItemSlotType.MainWeapon, new[]
                {
                    ItemType.Bow, ItemType.Sword, ItemType.Staff
                }
            },
            
            {
                ItemSlotType.SubWeapon, new[]
                {
                    ItemType.Shield
                }
            },
            
            {
                ItemSlotType.Cape, new[]
                {
                    ItemType.Cape
                }
            },
            
            {
                ItemSlotType.Top, new[]
                {
                    ItemType.Top
                }
            },
            
            {
                ItemSlotType.Pants, new[]
                {
                    ItemType.Pants
                }
            },
            
            {
                ItemSlotType.FaceAcc, new[]
                {
                    ItemType.FaceAcc
                }
            },
            
            {
                ItemSlotType.HairAcc, new[]
                {
                    ItemType.Hat
                }
            },
        };
        
        public static ItemType[] ToItemTypes(this ItemSlotType type) => itemSlotMappingData[type];
        
        public static ItemType[] GetMetaData(ItemSlotType type) => itemSlotMappingData[type];
    }
}
