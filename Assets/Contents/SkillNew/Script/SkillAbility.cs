using UnityEngine;

public class SkillAbility : Ability
{
    MainRealm mainRealm;
    MainRealmProcessorAbility mainRealmProcessorAbility;
    MainRealmProcessor mainRealmProcessor;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        mainRealm = Entity.GetParent<MainRealm>();
        mainRealmProcessorAbility = mainRealm?.GetAbility<MainRealmProcessorAbility>();
        mainRealmProcessor = mainRealmProcessorAbility?.GetProcessor<MainRealmProcessor>();
    }

    public Entity GetCaster()
    {
        return Entity;
    }

    public Vector3 GetTargetPosition(Tables.Skill skillData)
    {
        var caster = GetCaster() as Player;
        if (caster == null || skillData == null)
        {
            return Vector3.zero;
        }

        var targetPlayer = mainRealmProcessor?.GetClosestHostilePlayer(caster, skillData.range);
        if (targetPlayer == null)
        {
            return Vector3.zero;
        }

        return targetPlayer.transform.position;
    }

    public SkillContext CreateSkillContext(string skillKey)
    {
        var skillData = string.IsNullOrWhiteSpace(skillKey) ? null : Tables.Skill.Get(skillKey);
        var targetPosition = GetTargetPosition(skillData);
        var skillContext = new SkillContext
        {
            Caster = GetCaster(),
            SkillData = skillData,
            TargetPosition = targetPosition
        };

        skillContext.TargetEntities = SkillAreaLogic.GetTargetEntities(skillContext, mainRealmProcessor);
        return skillContext;
    }

    public bool TryUseSkill(string skillKey)
    {
        var skillData = string.IsNullOrWhiteSpace(skillKey) ? null : Tables.Skill.Get(skillKey);
        if (skillData == null)
        {
            return false;
        }

        var skillContext = CreateSkillContext(skillKey);
        SkillActionLogic.Execute(skillContext);

        return true;
    }
}
