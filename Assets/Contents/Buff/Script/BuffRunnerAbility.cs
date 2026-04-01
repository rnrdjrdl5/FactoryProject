using UnityEngine;

public class BuffRunnerAbility : Ability
{
    BuffContainer buffContainer = new();

    public void UseBuff(string buffKey)
    {
        // TODO: define duplicate/reapply policy when the same buffKey already exists at runtime.
        buffContainer.AddBuff(this, buffKey);
    }

    public void RemoveBuff(string buffKey)
    {
        buffContainer.RemoveBuff(buffKey);
    }

    public void ExpireBuff(string buffKey)
    {
        if (string.IsNullOrWhiteSpace(buffKey))
        {
            return;
        }

        Entity?.MessageBus?.Publish(new EntityDataMsg.BuffExpiredMsg
        {
            BuffRunnerAbility = this,
            BuffKey = buffKey
        });
    }

    void Update()
    {
        buffContainer.Update(Time.deltaTime);
    }
}

public static partial class EntityDataMsg
{
    public struct BuffExpiredMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.EntityData;
        public BuffRunnerAbility BuffRunnerAbility;
        public string BuffKey;
    }
}
