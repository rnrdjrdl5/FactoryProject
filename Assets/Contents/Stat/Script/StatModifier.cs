using System.Collections.Generic;

public class StatModifier : Tables.IStats
{
    public List<Tables.StatType> statTypes { get; set; } = new();
    public List<int> statValues { get; set; } = new();

    public bool TryGetStatValue(Tables.StatType statType, out int value)
    {
        value = 0;

        var index = statTypes.IndexOf(statType);
        if (index < 0)
        {
            return false;
        }

        value = statValues[index];
        return true;
    }
}
