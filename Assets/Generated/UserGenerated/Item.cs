using System;
using Unity.VisualScripting;

namespace Tables
{
    public partial class Item : IStats , IIconSprite, IGradeType , IDescription
    {
        public static ItemType[] ItemTypes => (ItemType[])Enum.GetValues(typeof(ItemType));
    }
}
