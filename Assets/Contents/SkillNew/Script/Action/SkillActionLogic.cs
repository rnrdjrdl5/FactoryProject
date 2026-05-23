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

            case SkillActionHealParam healParam:
                ExecuteHeal(skillContext, healParam);
                break;

            case SkillActionProjectileParam projectileParam:
                ExecuteProjectile(skillContext, projectileParam);
                break;

            case SkillActionAddBuffParam addBuffParam:
                ExecuteAddBuff(skillContext, addBuffParam);
                break;
        }
    }

    static void ExecuteDamage(SkillContext skillContext, SkillActionDamageParam damageParam)
    {
        if (skillContext.TargetEntities == null || skillContext.TargetEntities.Count == 0)
        {
            return;
        }

        var casterFormula = skillContext.OriginCaster?.GetEntityData<PlayerData>()?.PlayerFormula;
        var damage = SkillDamageLogic.GetDamage(casterFormula, damageParam);
        foreach (var targetEntity in skillContext.TargetEntities)
        {
            if (targetEntity == null)
            {
                continue;
            }

            var hpAbility = targetEntity.GetAbility<HpAbility>();
            hpAbility?.TryApplyDamage(skillContext.OriginCaster, damage);
        }
    }

    static void ExecuteHeal(SkillContext skillContext, SkillActionHealParam healParam)
    {
        if (skillContext.TargetEntities == null || skillContext.TargetEntities.Count == 0)
        {
            return;
        }

        var casterFormula = skillContext.OriginCaster?.GetEntityData<PlayerData>()?.PlayerFormula;
        var heal = SkillHealLogic.GetHeal(casterFormula, healParam);
        if (heal <= 0f)
        {
            return;
        }

        foreach (var targetEntity in skillContext.TargetEntities)
        {
            if (targetEntity == null)
            {
                continue;
            }

            var hpAbility = targetEntity.GetAbility<HpAbility>();
            if (hpAbility == null)
            {
                continue;
            }

            hpAbility.Heal(skillContext.OriginCaster, heal);
            SkillEffectLogic.Play(skillContext.SkillData.skillEffectKey, targetEntity);
        }
    }

    static void ExecuteProjectile(SkillContext skillContext, SkillActionProjectileParam projectileParam)
    {
        if (skillContext?.Caster == null || string.IsNullOrWhiteSpace(projectileParam?.ProjectileKey))
        {
            return;
        }

        var projectileData = Tables.Projectile.Get(projectileParam.ProjectileKey);
        if (projectileData == null || string.IsNullOrWhiteSpace(projectileData.prefabPath))
        {
            return;
        }

        var realm = skillContext.Caster.GetParent<Realm>();
        if (realm == null)
        {
            return;
        }

        var projectileEntity = realm.AddEntity<ProjectileEntity>(projectileData.prefabPath, new ProjectileInitData
        {
            ProjectileKey = projectileParam.ProjectileKey,
            SkillContext = skillContext,
            Position = skillContext.Caster.transform.position
        });
        if (projectileEntity == null)
        {
            return;
        }
    }

    static void ExecuteAddBuff(SkillContext skillContext, SkillActionAddBuffParam addBuffParam)
    {
        if (skillContext?.TargetEntities == null || skillContext.TargetEntities.Count == 0 || string.IsNullOrWhiteSpace(addBuffParam?.BuffKey))
        {
            return;
        }

        var buffData = Tables.Buff.Get(addBuffParam.BuffKey);
        if (buffData == null)
        {
            return;
        }

        var sourceKey = skillContext.SkillData?.Key;
        if (string.IsNullOrWhiteSpace(sourceKey))
        {
            return;
        }

        foreach (var targetEntity in skillContext.TargetEntities)
        {
            var targetPlayerData = targetEntity?.GetEntityData<PlayerData>();
            targetPlayerData?.Buff?.AddBuff(addBuffParam.BuffKey, sourceKey);
        }
    }
}
