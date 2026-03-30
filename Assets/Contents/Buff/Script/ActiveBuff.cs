using UnityEngine;

public class ActiveBuff
{
    BuffAbility buffAbility;
    SkillAbility skillAbility;
    string buffKey;
    Tables.Buff buffData => Tables.Buff.Get(buffKey);
    float remainDuration;
    int stack;
    float tickTimer;

    public static ActiveBuff Create(BuffAbility buffAbility, string buffKey)
    {
        var activeBuff = new ActiveBuff();
        activeBuff.Initialize(buffAbility, buffKey);
        return activeBuff;
    }

    public void Initialize(BuffAbility buffAbility, string buffKey)
    {
        this.buffAbility = buffAbility;
        this.buffKey = buffKey;
        skillAbility = buffAbility?.Entity?.GetAbility<SkillAbility>();
        remainDuration = buffData?.duration ?? 0f;
        stack = 1;
        tickTimer = 0f;

        if (!string.IsNullOrWhiteSpace(buffData?.startSkillKey))
        {
            skillAbility?.TryUseSkill(buffData.startSkillKey);
        }
    }

    public bool Update(float deltaTime)
    {
        remainDuration -= deltaTime;
        return remainDuration <= 0f;
    }

    public void Uninitialize()
    {
        if (!string.IsNullOrWhiteSpace(buffData?.endSkillKey))
        {
            skillAbility?.TryUseSkill(buffData.endSkillKey);
        }
    }
}
