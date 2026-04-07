public class DetectHostileTargetFlow : ProcessorFlow
{
    float detectionRange = 6f;
    float targetLostRange = 8f;
    float followDistance = 1.5f;

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

        var brain = Entity as Brain;
        controlledPlayer = brain?.Controll as Player;
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
            followAbility?.SetFollowDistance(followDistance);

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
        if (TargetSearchLogic.IsHostileTargetInRange(controlledPlayer, targetPlayer, targetLostRange))
        {
            return targetPlayer;
        }

        return TargetSearchLogic.GetClosestHostilePlayer(
            Realm.GetChildren<Player>(),
            controlledPlayer,
            controlledPlayer.transform.position,
            detectionRange);
    }
}
