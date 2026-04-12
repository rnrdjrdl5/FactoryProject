namespace Tables
{
    public static partial class EnumLogic
    {
        public static PixemPartType ToPixemPartType(this ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Bow:
                    return PixemPartType.RightHandWeapon;
                case ItemType.Shield:
                    return PixemPartType.Shield;
                case ItemType.Sword:
                    return PixemPartType.LeftHandWeapon;
                case ItemType.Staff:
                    return PixemPartType.LeftHandWeapon;
                case ItemType.Top:
                    return PixemPartType.Top;
                case ItemType.Pants:
                    return PixemPartType.Pants;
                case ItemType.Cape:
                    return PixemPartType.Cape;
                case ItemType.FaceAcc:
                    return PixemPartType.FaceAcc2;
                case ItemType.Hat:
                    return PixemPartType.HairAcc;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(itemType), itemType, "itemType cannot be converted to PixemPartType.");
            }
        }
    }
}