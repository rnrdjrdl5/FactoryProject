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

    public Vector3 GetTargetPosition(Tables.Skill skillData, Entity factionEntity)
    {
        var factionPlayer = factionEntity as Player;
        if (factionPlayer == null || Entity == null || skillData == null)
        {
            return Vector3.zero;
        }

        var targetPlayer = mainRealmProcessor?.GetClosestHostilePlayer(factionPlayer, Entity.transform.position, skillData.range);
        if (targetPlayer == null)
        {
            return Vector3.zero;
        }

        return targetPlayer.transform.position;
    }

    public SkillContext CreateSkillContext(string skillKey, SkillContext parentContext = null)
    {
        var skillData = string.IsNullOrWhiteSpace(skillKey) ? null : Tables.Skill.Get(skillKey);
        var originCaster = parentContext?.OriginCaster ?? Entity;
        var targetPosition = GetTargetPosition(skillData, originCaster);
        
        var skillContext = SkillContext.Create(
            parentContext,
            originCaster,
            Entity,
            skillData,
            targetPosition);

        skillContext.TargetEntities = SkillAreaLogic.GetTargetEntities(skillContext, mainRealmProcessor);
        return skillContext;
    }

    public bool TryUseSkill(string skillKey, SkillContext parentContext = null)
    {
        var skillData = string.IsNullOrWhiteSpace(skillKey) ? null : Tables.Skill.Get(skillKey);
        if (skillData == null)
        {
            return false;
        }

        var skillContext = CreateSkillContext(skillKey, parentContext);
        SkillActionLogic.Execute(skillContext);

        return true;
    }
}
