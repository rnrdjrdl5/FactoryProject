using UnityEngine;

public class CombatHostileTargetFlow : ProcessorFlow
{
    float attackRange = 1.5f;
    float targetLostRange = 8f;
    float followDistance = 1.5f;

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
    }

    public override void OnUpdateFlow()
    {
        var targetPlayer = followAbility?.TargetPlayer;
        if (!TargetSearchLogic.IsHostileTargetInRange(controlledPlayer, targetPlayer, targetLostRange))
        {
            followAbility?.ClearTarget();
            parent?.ActivateChildFlow<WanderFlow>();
            return;
        }

        followAbility?.SetFollowDistance(followDistance);

        var distance = Vector2.Distance(controlledPlayer.transform.position, targetPlayer.transform.position);
        if (distance <= attackRange)
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
}
