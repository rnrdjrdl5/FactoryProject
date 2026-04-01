public static class BuffLogic
{
    public static void ApplyOnStart(ActiveBuff activeBuff)
    {
        if (activeBuff?.BuffData?.ParsedEffectParam == null)
        {
            return;
        }

        switch (activeBuff.BuffData.ParsedEffectParam)
        {
            case BuffEffectStatModifierParam statModifierParam:
                ApplyStatModifier(activeBuff, statModifierParam);
                break;
        }
    }

    public static void ApplyOnEnd(ActiveBuff activeBuff)
    {
        if (activeBuff?.BuffData?.ParsedEffectParam == null)
        {
            return;
        }

        switch (activeBuff.BuffData.ParsedEffectParam)
        {
            case BuffEffectStatModifierParam:
                RemoveStatModifier(activeBuff);
                break;

            case BuffEffectUseSkillOnEndParam useSkillOnEndParam:
                UseSkillOnEnd(activeBuff, useSkillOnEndParam);
                break;
        }
    }

    static void ApplyStatModifier(ActiveBuff activeBuff, BuffEffectStatModifierParam statModifierParam)
    {
        var stat = GetStat(activeBuff);
        var statModifier = statModifierParam?.CreateStatModifier();
        if (stat == null || statModifier == null)
        {
            return;
        }

        stat.AddStats(GetSourceKey(activeBuff), statModifier);
    }

    static void RemoveStatModifier(ActiveBuff activeBuff)
    {
        var stat = GetStat(activeBuff);
        if (stat == null)
        {
            return;
        }

        stat.RemoveStats(GetSourceKey(activeBuff));
    }

    static void UseSkillOnEnd(ActiveBuff activeBuff, BuffEffectUseSkillOnEndParam useSkillOnEndParam)
    {
        if (string.IsNullOrWhiteSpace(useSkillOnEndParam?.SkillKey))
        {
            return;
        }

        var skillAbility = activeBuff.BuffRunnerAbility?.Entity?.GetAbility<SkillAbility>();
        skillAbility?.TryUseSkill(useSkillOnEndParam.SkillKey);
    }

    static Stat GetStat(ActiveBuff activeBuff)
    {
        return activeBuff?.BuffRunnerAbility?.Entity?.GetEntityData<PlayerData>()?.Stat;
    }

    static StatSourceKey GetSourceKey(ActiveBuff activeBuff)
    {
        return new StatSourceKey(StatSourceType.Buff, activeBuff?.BuffKey);
    }
}
