using System.Collections.Generic;

public class PlayerBuffProcessor : Processor
{
    PlayerData playerData;
    BuffRunnerAbility buffRunnerAbility;

    public override void Ready()
    {
        base.Ready();

        playerData = Entity.GetEntityData<PlayerData>();
        buffRunnerAbility = Entity.GetAbility<BuffRunnerAbility>();
        playerData?.MessageBus?.Subscribe<EntityDataMsg.BuffChangedMsg>(OnBuffChanged);
        playerData?.MessageBus?.Subscribe<EntityDataMsg.BuffExpiredMsg>(OnBuffExpired);
        InitializeBuffs();
    }

    public override void Uninitialize()
    {
        ClearRuntimeBuffs();
        playerData?.MessageBus?.Unsubscribe<EntityDataMsg.BuffChangedMsg>(OnBuffChanged);
        playerData?.MessageBus?.Unsubscribe<EntityDataMsg.BuffExpiredMsg>(OnBuffExpired);

        base.Uninitialize();
    }

    void InitializeBuffs()
    {
        if (playerData?.Buff == null || buffRunnerAbility == null)
        {
            return;
        }

        foreach (var buffKey in playerData.Buff.BuffValuesByKey.Keys)
        {
            buffRunnerAbility.UseBuff(buffKey);
        }
    }

    void OnBuffChanged(EntityDataMsg.BuffChangedMsg msg)
    {
        if (playerData?.Buff == null || buffRunnerAbility == null || msg.Buff != playerData.Buff)
        {
            return;
        }

        if (msg.IsRemoved)
        {
            buffRunnerAbility.RemoveBuff(msg.BuffKey);
            return;
        }

        buffRunnerAbility.UseBuff(msg.BuffKey);
    }

    void OnBuffExpired(EntityDataMsg.BuffExpiredMsg msg)
    {
        if (playerData?.Buff == null || buffRunnerAbility == null || msg.BuffRunnerAbility != buffRunnerAbility)
        {
            return;
        }

        if (!playerData.Buff.TryGetBuffValue(msg.BuffKey, out var buffValue))
        {
            buffRunnerAbility.RemoveBuff(msg.BuffKey);
            return;
        }

        if (buffValue.BuffLifetimeType == BuffLifetimeType.Runtime)
        {
            playerData.Buff.RemoveBuff(msg.BuffKey);
            return;
        }

        buffRunnerAbility.RemoveBuff(msg.BuffKey);
    }

    void ClearRuntimeBuffs()
    {
        if (playerData?.Buff == null)
        {
            return;
        }

        var buffKeys = new List<string>(playerData.Buff.BuffValuesByKey.Keys);
        foreach (var buffKey in buffKeys)
        {
            if (!playerData.Buff.TryGetBuffValue(buffKey, out var buffValue))
            {
                continue;
            }

            if (buffValue.BuffLifetimeType != BuffLifetimeType.Runtime)
            {
                continue;
            }

            playerData.Buff.RemoveBuff(buffKey);
        }
    }
}
