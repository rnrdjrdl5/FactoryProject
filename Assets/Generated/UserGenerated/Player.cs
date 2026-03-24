using System.Collections.Generic;
using System.Linq;

namespace Tables
{
    public partial class Player : IStats , IIconSprite, IGradeType, IKey , IPrefabPath
    {
        public static Player GetPlayerByItemKey(string itemKey)
        {
            return Table.Values.FirstOrDefault(value => value.dropPlayerKey == itemKey);
        }

        public IEnumerable<(StatType, int)> GetStatTuple()
        {
            return statTypes.Zip(statValues, (type, value) => (type, value));
        }
    }
}
