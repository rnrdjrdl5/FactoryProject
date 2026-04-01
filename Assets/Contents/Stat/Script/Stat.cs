using System.Collections.Generic;
using Newtonsoft.Json;

public class Stat : IEntityData, IMessageBus
{
    [JsonIgnore] public MessageBus MessageBus { get; set; }
    [JsonProperty] StatModifier totalStat = new();
    [JsonIgnore] Dictionary<StatSourceKey, Tables.IStats> appliedStats = new();
    
    public void Initialize(IInitData initData = null)
    {
        
    }

    public void Uninitialize()
    {
        
    }

    public void OnSetMessageBus()
    {
        
    }

    public void ClearStats()
    {
        appliedStats.Clear();
        RefreshStats();
        PublishStatChanged(new StatSourceKey(StatSourceType.None, string.Empty));
    }

    public void AddStats(StatSourceKey sourceKey, Tables.IStats stats)
    {
        if (stats == null)
        {
            return;
        }

        appliedStats[sourceKey] = stats;
        RefreshStats();
        PublishStatChanged(sourceKey);
    }
    
    public void RemoveStats(StatSourceKey sourceKey)
    {
        if (!appliedStats.ContainsKey(sourceKey))
        {
            return;
        }

        appliedStats.Remove(sourceKey);
        RefreshStats();
        PublishStatChanged(sourceKey);
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

        foreach (var appliedStat in appliedStats.Values)
        {
            totalStat.MergeStat(appliedStat);
        }
    }

    void PublishStatChanged(StatSourceKey sourceKey)
    {
        MessageBus?.Publish(new EntityDataMsg.StatChangedMsg
        {
            Stat = this,
            SourceKey = sourceKey
        });
    }
}

public static partial class EntityDataMsg
{
    public struct StatChangedMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.EntityData;
        public Stat Stat;
        public StatSourceKey SourceKey;
    }
}
