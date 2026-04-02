using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffProcessor : UpdateProcessor
{
    PlayerData playerData;
    BuffAbility buffAbility;
    readonly List<string> expiredBuffKeys = new();

    public override void Ready()
    {
        base.Ready();

        playerData = Entity.GetEntityData<PlayerData>();
        buffAbility = Entity.GetAbility<BuffAbility>();
        playerData?.MessageBus?.Subscribe<EntityDataMsg.BuffChangedMsg>(OnBuffChanged);
        InitializeBuffs();
    }

    public override void Uninitialize()
    {
        ClearRuntimeBuffs();
        playerData?.MessageBus?.Unsubscribe<EntityDataMsg.BuffChangedMsg>(OnBuffChanged);

        base.Uninitialize();
    }

    public override void Update()
    {
        base.Update();

        if (playerData?.Buff == null)
        {
            return;
        }

        expiredBuffKeys.Clear();
        playerData.Buff.UpdateRuntimeBuffs(Time.deltaTime, expiredBuffKeys);
        foreach (var buffKey in expiredBuffKeys)
        {
            playerData.Buff.RemoveBuff(buffKey);
        }
    }

    void InitializeBuffs()
    {
        if (playerData?.Buff == null || buffAbility == null)
        {
            return;
        }

        foreach (var buffKey in playerData.Buff.BuffValuesByKey.Keys)
        {
            if (!playerData.Buff.TryGetBuffValue(buffKey, out var buffValue))
            {
                continue;
            }

            buffAbility.UseBuff(buffKey);
        }
    }

    void OnBuffChanged(EntityDataMsg.BuffChangedMsg msg)
    {
        if (playerData?.Buff == null || buffAbility == null || msg.Buff != playerData.Buff)
        {
            return;
        }

        if (msg.IsRemoved)
        {
            buffAbility.RemoveBuff(msg.BuffKey);
            return;
        }

        buffAbility.UseBuff(msg.BuffKey);
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
