using System.Collections.Generic;
using Newtonsoft.Json;

public class Stat : IEntityData
{
    [JsonProperty] EntityStat totalStat = new();
    [JsonIgnore] List<Tables.IStats> appliedStats = new();
    
    public void Initialize(IInitData initData = null)
    {
        
    }

    public void Uninitialize()
    {
        
    }

    public void ClearStats()
    {
        appliedStats.Clear();
        RefreshStats();
    }

    public void AddStats(Tables.IStats stats)
    {
        if (stats == null || appliedStats.Contains(stats))
        {
            return;
        }
        
        appliedStats.Add(stats);
        RefreshStats();
    }
    
    public void RemoveStats(Tables.IStats stats)
    {
        if (!appliedStats.Contains(stats))
        {
            return;
        }
        
        appliedStats.Remove(stats);
        RefreshStats();
    }

    public bool TryGetStat(Tables.StatType statType, out int value)
    {
        return totalStat.TryGetStatValue(statType, out value);
    }
    
    void RefreshStats()
    {
        totalStat.statTypes ??= new List<Tables.StatType>();
        totalStat.statValues ??= new List<int>();
        totalStat.statTypes.Clear();
        totalStat.statValues.Clear();

        foreach (var appliedStat in appliedStats)
        {
            totalStat.MergeStat(appliedStat);
        }
    }
}

public class EntityStat : Tables.IStats
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
