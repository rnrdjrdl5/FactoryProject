using System.Linq;

namespace Tables
{
    public partial class FactionRelation
    {
        public static FactionRelationType GetRelation(FactionType from, FactionType to)
        {
            var targetData = _table.FirstOrDefault(kv => kv.Value.factionType == from).Value;
            var factionRelationType = targetData.factionTypes[(int)to - 1];

            return factionRelationType;
        }
    }
}
