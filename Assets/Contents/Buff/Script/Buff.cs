using System.Collections.Generic;
using Newtonsoft.Json;

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

    public void SetBuff(
        string buffKey,
        string sourceKey,
        BuffLifetimeType buffLifetimeType = BuffLifetimeType.Runtime)
    {
        if (string.IsNullOrWhiteSpace(sourceKey) || string.IsNullOrWhiteSpace(buffKey))
        {
            return;
        }

        buffValuesByKey[buffKey] = new BuffValue
        {
            BuffKey = buffKey,
            SourceKey = sourceKey,
            BuffLifetimeType = buffLifetimeType
        };
        PublishBuffChanged(buffKey, sourceKey, buffLifetimeType, false);
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
            PublishBuffChanged(buffKey, buffValue.SourceKey, buffValue.BuffLifetimeType, true);
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

    void PublishBuffChanged(
        string buffKey,
        string sourceKey,
        BuffLifetimeType buffLifetimeType,
        bool isRemoved)
    {
        MessageBus?.Publish(new EntityDataMsg.BuffChangedMsg
        {
            Buff = this,
            BuffKey = buffKey,
            SourceKey = sourceKey,
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
        public BuffLifetimeType BuffLifetimeType;
        public bool IsRemoved;
    }
}
