using System.Collections.Generic;

public class Stat : IEntityData
{
    EntityStat totalStat = new();
    List<Tables.IStats> appliedStats = new();
    
    public void Initialize(IInitData initData = null)
    {
        
    }

    public void Uninitialize()
    {
        
    }

    public void AddStats(Tables.IStats stats)
    {
        if (appliedStats.Contains(stats))
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

    void RefreshStats()
    {
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
}