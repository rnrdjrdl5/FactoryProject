using Tables;
using UnityEngine;

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

            case SkillActionProjectileParam projectileParam:
                ExecuteProjectile(skillContext, projectileParam);
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

    static void ExecuteProjectile(SkillContext skillContext, SkillActionProjectileParam projectileParam)
    {
        if (skillContext?.Caster == null || string.IsNullOrWhiteSpace(projectileParam?.PrefabPath))
        {
            return;
        }

        var realm = skillContext.Caster.GetParent<Realm>();
        if (realm == null)
        {
            return;
        }

        var projectileEntity = realm.AddEntity<ProjectileEntity>(projectileParam.PrefabPath, new ProjectileInitData
        {
            SkillContext = skillContext,
            Position = skillContext.Caster.transform.position
        });
        if (projectileEntity == null)
        {
            return;
        }
    }
}
