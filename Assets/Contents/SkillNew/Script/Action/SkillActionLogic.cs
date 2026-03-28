using Tables;

public static class SkillActionLogic
{
    public static void Execute(SkillContext skillContext)
    {
        if (skillContext?.SkillData == null)
        {
            return;
        }

        switch (skillContext.SkillData.ParsedActionParam)
        {
            case SkillActionDamageParam damageParam:
                ExecuteDamage(skillContext, damageParam);
                break;
        }
    }

    static void ExecuteDamage(SkillContext skillContext, SkillActionDamageParam damageParam)
    {
        if (skillContext.TargetEntities == null || skillContext.TargetEntities.Count == 0)
        {
            return;
        }

        var damage = GetDamage(skillContext, damageParam);
        foreach (var targetEntity in skillContext.TargetEntities)
        {
            if (targetEntity == null)
            {
                continue;
            }

            var hpAbility = targetEntity.GetAbility<HpAbility>();
            hpAbility?.TryApplyDamage(skillContext.Caster, damage);
        }
    }

    static float GetDamage(SkillContext skillContext, SkillActionDamageParam damageParam)
    {
        if (damageParam.Amount != null)
        {
            return damageParam.Amount.Value;
        }

        var casterPlayerData = skillContext.Caster?.GetEntityData<PlayerData>();
        return DamageLogic.GetDamage(casterPlayerData);
    }
}
