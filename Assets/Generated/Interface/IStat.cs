using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tables
{
    public interface IStats
    {
        List<StatType> statTypes { get; set; }
        List<int> statValues { get; set; }

        public void SetFormulaParameter(FormulaParameter formulaParameter)
        {
            formulaParameter.ClearParameter();
            
            for (int i = 0; i < statTypes.Count; i++)
            {
                var statType = statTypes[i];
                formulaParameter.SetParameter(EnumLogic.GetStatName(statType), () => statValues[i]);
            }
        }
    }
}

public static class IStatsEx
{
    public static void MergeStat(this Tables.IStats stat, Tables.IStats other)
    {
        for (int i = 0; i < other.statTypes.Count; i++)
        {
            var otherType = other.statTypes[i];
            var otherValue = other.statValues[i];

            var index = stat.statTypes.IndexOf(otherType);

            if (index >= 0)
            {
                stat.statValues[index] += otherValue;
            }
            else
            {
                stat.statTypes.Add(otherType);
                stat.statValues.Add(otherValue);
            }
        }
    }
}