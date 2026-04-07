using UnityEngine;

public class CombatHostileTargetFlow : ProcessorFlow
{
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

        controlledPlayer = EnemyAIFlowLogic.GetControlledPlayer(this);
        followAbility = controlledPlayer?.GetAbility<PlayerFollowAbility>();
    }

    public override void OnUpdateFlow()
    {
        var targetPlayer = followAbility?.TargetPlayer;
        if (!EnemyAIFlowLogic.IsTargetValid(controlledPlayer, targetPlayer, EnemyAIFlowLogic.GetTargetLostRange(controlledPlayer)))
        {
            followAbility?.ClearTarget();
            parent?.ActivateChildFlow<WanderFlow>();
            return;
        }

        EnemyAIFlowLogic.SyncFollowDistanceToAttackRange(controlledPlayer, followAbility);

        var attackRange = EnemyAIFlowLogic.GetMainAttackRange(controlledPlayer);
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
