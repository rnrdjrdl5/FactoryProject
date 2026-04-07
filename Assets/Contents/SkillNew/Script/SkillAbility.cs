using UnityEngine;
using System.Collections.Generic;

public class SkillAbility : Ability
{
    readonly Dictionary<string, SkillRuntimeState> skillRuntimeStatesByKey = new();

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

    void Update()
    {
        foreach (var state in skillRuntimeStatesByKey.Values)
        {
            state.UpdateCooldown(Time.deltaTime);
        }
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

    SkillRuntimeState GetOrCreateSkillRuntimeState(string skillKey)
    {
        if (!skillRuntimeStatesByKey.TryGetValue(skillKey, out var state))
        {
            state = SkillRuntimeState.Create(skillKey);
            skillRuntimeStatesByKey[skillKey] = state;
        }

        return state;
    }

    public bool TryUseSkill(string skillKey, SkillContext parentContext = null)
    {
        var skillData = string.IsNullOrWhiteSpace(skillKey) ? null : Tables.Skill.Get(skillKey);
        if (skillData == null)
        {
            return false;
        }

        var skillRuntimeState = GetOrCreateSkillRuntimeState(skillKey);
        if (!skillRuntimeState.CanUse())
        {
            return false;
        }

        var skillContext = CreateSkillContext(skillKey, parentContext);
        SkillActionLogic.Execute(skillContext);
        skillRuntimeState.SetCooldown(skillData.cooldown);

        return true;
    }
}
