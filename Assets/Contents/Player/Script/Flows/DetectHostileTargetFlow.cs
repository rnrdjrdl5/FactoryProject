public class DetectHostileTargetFlow : ProcessorFlow
{
    Player controlledPlayer;
    PlayerFollowAbility followAbility;

    public override void OnAddFlow()
    {
        base.OnAddFlow();

        AddChild<WanderFlow>(Processor);
        AddChild<CombatHostileTargetFlow>(Processor);
    }

    public override void OnEnterFlow()
    {
        base.OnEnterFlow();

        controlledPlayer = EnemyAIFlowLogic.GetControlledPlayer(this);
        followAbility = controlledPlayer?.GetAbility<PlayerFollowAbility>();
    }

    public override void OnUpdateFlow()
    {
        var targetPlayer = ResolveTargetPlayer();

        if (targetPlayer == null)
        {
            followAbility?.ClearTarget();

            if (!IsActivateFlow<WanderFlow>())
            {
                ActivateChildFlow<WanderFlow>();
            }
        }
        else
        {
            followAbility?.SetTarget(targetPlayer);
            EnemyAIFlowLogic.SyncFollowDistanceToAttackRange(controlledPlayer, followAbility);

            if (!IsActivateFlow<CombatHostileTargetFlow>())
            {
                ActivateChildFlow<CombatHostileTargetFlow>();
            }
        }

        base.OnUpdateFlow();
    }

    Player ResolveTargetPlayer()
    {
        if (controlledPlayer == null)
        {
            return null;
        }

        var targetPlayer = followAbility?.TargetPlayer;
        var targetLostRange = EnemyAIFlowLogic.GetTargetLostRange(controlledPlayer);
        if (EnemyAIFlowLogic.IsTargetValid(controlledPlayer, targetPlayer, targetLostRange))
        {
            return targetPlayer;
        }

        var detectionRange = EnemyAIFlowLogic.GetDetectionRange(controlledPlayer);
        return EnemyAIFlowLogic.FindClosestHostileTarget(this, controlledPlayer, detectionRange);
    }
}
