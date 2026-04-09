using UnityEngine;

public class CombatHostileTargetFlow : ProcessorFlow
{
    float maxChaseDistance = 8f;
    float maxChaseDuration = 5f;
    float followDistance = 1.5f;
    float mainSkillRange = 1.5f;

    Player controlledPlayer;
    PlayerFollowAbility followAbility;

    public override void OnAddFlow()
    {
        base.OnAddFlow();

        AddChild<FollowTargetFlow>(Processor);
        AddChild<AutoAttackFlow>(Processor);
    }

    public override void OnEnterFlow()
    {
        base.OnEnterFlow();

        var brain = Entity as Brain;
        controlledPlayer = brain?.Controll as Player;
        followAbility = controlledPlayer?.GetAbility<PlayerFollowAbility>();
        mainSkillRange = ResolveMainSkillRange();
    }

    public override void OnUpdateFlow()
    {
        var targetPlayer = followAbility?.TargetPlayer;
        if (!TargetSearchLogic.IsHostileTargetInRange(controlledPlayer, targetPlayer, maxChaseDistance) ||
            elapsedTime >= maxChaseDuration)
        {
            followAbility?.ClearTarget();
            parent?.ActivateChildFlow<WanderFlow>();
            return;
        }

        followAbility?.SetFollowDistance(followDistance);

        var distance = Vector2.Distance(controlledPlayer.transform.position, targetPlayer.transform.position);
        if (distance <= mainSkillRange)
        {
            if (!IsActivateFlow<AutoAttackFlow>())
            {
                ActivateChildFlow<AutoAttackFlow>();
            }
        }
        else if (!IsActivateFlow<FollowTargetFlow>())
        {
            ActivateChildFlow<FollowTargetFlow>();
        }

        base.OnUpdateFlow();
    }

    float ResolveMainSkillRange()
    {
        var playerData = controlledPlayer?.GetEntityData<PlayerData>();
        if (!InputActionSkillLogic.TryGetSkillKey(playerData, InputActionType.MainAttack, out var skillKey))
        {
            return mainSkillRange;
        }

        var skillData = Tables.Skill.Get(skillKey);
        if (skillData == null)
        {
            return mainSkillRange;
        }

        return skillData.range;
    }
}
