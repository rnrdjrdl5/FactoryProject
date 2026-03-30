using System.Collections.Generic;

public class BuffContainer
{
    Dictionary<string, ActiveBuff> activeBuffs = new();

    public void AddBuff(BuffAbility buffAbility, string buffKey)
    {
        activeBuffs[buffKey] = ActiveBuff.Create(buffAbility, buffKey);
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

            activeBuff.Uninitialize();
            activeBuffs.Remove(buffKey);
        }
    }
}
