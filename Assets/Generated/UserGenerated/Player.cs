using System.Linq;

namespace Tables
{
    public partial class Player : IStats , IIconSprite, IGradeType, IKey , IPrefabPath
    {
        public static Player GetPlayerByItemKey(string itemKey)
        {
            return Table.Values.FirstOrDefault(value => value.dropPlayerKey == itemKey);
        }
    }
}
