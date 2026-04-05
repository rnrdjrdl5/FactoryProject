using Newtonsoft.Json;
using System.Collections.Generic;

public class Buff : IEntityData, IMessageBus
{
    [JsonIgnore] public MessageBus MessageBus { get; set; }
    public IReadOnlyDictionary<string, BuffValue> BuffValuesByKey => buffValuesByKey;

    [JsonProperty] Dictionary<string, BuffValue> buffValuesByKey = new();

    public void Initialize(IInitData initData = null)
    {
        buffValuesByKey.Clear();
    }

    public void Uninitialize()
    {
        buffValuesByKey.Clear();
    }

    public void OnSetMessageBus()
    {
    }

    public void AddBuff(
        string buffKey,
        string sourceKey,
        BuffLifetimeType buffLifetimeType = BuffLifetimeType.Runtime,
        float remainTime = float.NaN)
    {
        if (string.IsNullOrWhiteSpace(sourceKey) || string.IsNullOrWhiteSpace(buffKey))
        {
            return;
        }

        if (float.IsNaN(remainTime))
        {
            remainTime = GetInitialRemainTime(buffKey, buffLifetimeType);
        }

        buffValuesByKey[buffKey] = new BuffValue
        {
            BuffKey = buffKey,
            SourceKey = sourceKey,
            RemainTime = remainTime,
            BuffLifetimeType = buffLifetimeType
        };
        PublishBuffChanged(buffKey, sourceKey, remainTime, buffLifetimeType, false);
    }

    public bool RemoveBuff(string buffKey)
    {
        if (string.IsNullOrWhiteSpace(buffKey))
        {
            return false;
        }

        if (!buffValuesByKey.TryGetValue(buffKey, out var buffValue))
        {
            return false;
        }

        var removed = buffValuesByKey.Remove(buffKey);
        if (removed)
        {
            PublishBuffChanged(buffKey, buffValue.SourceKey, buffValue.RemainTime, buffValue.BuffLifetimeType, true);
        }

        return removed;
    }

    public bool TryGetBuffValue(string buffKey, out BuffValue buffValue)
    {
        buffValue = default;
        if (string.IsNullOrWhiteSpace(buffKey))
        {
            return false;
        }

        return buffValuesByKey.TryGetValue(buffKey, out buffValue);
    }

    public void UpdateRuntimeBuffs(float deltaTime, List<string> expiredBuffKeys)
    {
        if (deltaTime <= 0f)
        {
            return;
        }

        var buffKeys = new List<string>(buffValuesByKey.Keys);
        foreach (var buffKey in buffKeys)
        {
            if (!buffValuesByKey.TryGetValue(buffKey, out var buffValue))
            {
                continue;
            }

            if (buffValue.BuffLifetimeType != BuffLifetimeType.Runtime)
            {
                continue;
            }

            var buffData = Tables.Buff.Get(buffKey);
            if (buffData == null || buffData.IsInfiniteDuration)
            {
                continue;
            }

            buffValue.RemainTime -= deltaTime;
            buffValuesByKey[buffKey] = buffValue;
            if (buffValue.RemainTime > 0f)
            {
                continue;
            }

            expiredBuffKeys?.Add(buffKey);
        }
    }

    float GetInitialRemainTime(string buffKey, BuffLifetimeType buffLifetimeType)
    {
        if (buffLifetimeType != BuffLifetimeType.Runtime)
        {
            return -1f;
        }

        return Tables.Buff.Get(buffKey)?.duration ?? -1f;
    }

    void PublishBuffChanged(
        string buffKey,
        string sourceKey,
        float remainTime,
        BuffLifetimeType buffLifetimeType,
        bool isRemoved)
    {
        MessageBus?.Publish(new EntityDataMsg.BuffChangedMsg
        {
            Buff = this,
            BuffKey = buffKey,
            SourceKey = sourceKey,
            RemainTime = remainTime,
            BuffLifetimeType = buffLifetimeType,
            IsRemoved = isRemoved
        });
    }
}

public enum BuffLifetimeType
{
    External = 0,
    Runtime = 1
}

public struct BuffValue
{
    public string BuffKey;
    public string SourceKey;
    public float RemainTime;
    public BuffLifetimeType BuffLifetimeType;
}

public static partial class EntityDataMsg
{
    public struct BuffChangedMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.EntityData;
        public Buff Buff;
        public string BuffKey;
        public string SourceKey;
        public float RemainTime;
        public BuffLifetimeType BuffLifetimeType;
        public bool IsRemoved;
    }
}
