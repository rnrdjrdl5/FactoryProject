using System.Collections.Generic;

public class BuffContainer
{
    Dictionary<string, ActiveBuff> activeBuffs = new();

    public void AddBuff(BuffAbility buffAbility, string buffKey)
    {
        activeBuffs[buffKey] = ActiveBuff.Create(buffAbility, buffKey);
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
}
