using System.Collections.Generic;

public class BuffContainer
{
    Dictionary<string, ActiveBuff> activeBuffs = new();

    public void AddBuff(BuffRunnerAbility buffRunnerAbility, string buffKey)
    {
        activeBuffs[buffKey] = ActiveBuff.Create(buffRunnerAbility, buffKey);
    }

    public void RemoveBuff(string buffKey)
    {
        if (!activeBuffs.TryGetValue(buffKey, out var activeBuff))
        {
            return;
        }

        activeBuff.Uninitialize();
        activeBuffs.Remove(buffKey);
    }

    public void Update(float deltaTime)
    {
        var buffKeys = new List<string>(activeBuffs.Keys);
        foreach (var buffKey in buffKeys)
        {
            if (!activeBuffs.TryGetValue(buffKey, out var activeBuff))
            {
                continue;
            }

            if (!activeBuff.Update(deltaTime))
            {
                continue;
            }

            activeBuff.BuffRunnerAbility.ExpireBuff(buffKey);
        }
    }
}
