using UnityEngine;

public class BrainActionProcessor : Processor
{
    Brain brain;
    BrainActionExecutionContext executionContext;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        executionContext = new BrainActionExecutionContext();

        brain = Entity as Brain;
        if (brain == null)
        {
            return;
        }

        brain.OnAttachControll += RefreshControlledCache;
        brain.OnDetachControll += ClearControlledCache;

        RefreshControlledCache(brain.Controll);
    }

    public override void Ready()
    {
        base.Ready();
    }

    public override void Uninitialize()
    {
        if (brain != null)
        {
            brain.OnAttachControll -= RefreshControlledCache;
            brain.OnDetachControll -= ClearControlledCache;
        }

        ResetControlledCache();
        executionContext = null;

        base.Uninitialize();
    }

    public bool RequestAction(BrainActionRequest request)
    {
        switch (request.RequestType)
        {
            case BrainActionRequestType.Move:
                return Move(request.Direction);
            case BrainActionRequestType.Pick:
                return Pick();
            case BrainActionRequestType.UseSkill:
                return UseSkill(request.KeyCode);
            case BrainActionRequestType.Intent:
                return ExecuteIntentAction(request);
            default:
                return false;
        }
    }

    bool ExecuteIntentAction(BrainActionRequest request)
    {
        switch (request.IntentActionType)
        {
            case IntentActionType.FollowTarget:
                return FollowTarget();
            case IntentActionType.UseMainAttackSkill:
                return UseSkill(KeyCode.Mouse0);
            default:
                return false;
        }
    }

    bool Move(Vector2 direction)
    {
        if (executionContext?.MoveAbility == null)
        {
            return false;
        }

        var moveDelta = executionContext.MoveAbility.Move(direction);
        RefreshMoveAnimation(moveDelta);
        return true;
    }

    bool FollowTarget()
    {
        if (executionContext?.FollowAbility == null)
        {
            return false;
        }

        var moveDelta = executionContext.FollowAbility.Move();
        RefreshMoveAnimation(moveDelta);
        return true;
    }

    bool Pick()
    {
        if (executionContext?.PickProcessor == null)
        {
            return false;
        }

        executionContext.PickProcessor.PickItem();
        return true;
    }

    bool UseSkill(KeyCode keyCode)
    {
        var playerData = executionContext?.ControlledEntity?.GetEntityData<PlayerData>();
        if (!InputActionSkillLogic.TryGetSkillKey(playerData, keyCode, out var skillKey))
        {
            return false;
        }

        return UseSkill(skillKey);
    }

    bool UseSkill(string skillKey)
    {
        if (executionContext?.SkillAbility == null)
        {
            return false;
        }

        return executionContext.SkillAbility.TryUseSkill(skillKey);
    }

    void RefreshMoveAnimation(Vector2 moveDelta)
    {
        if (executionContext?.ModelProcessor == null)
        {
            return;
        }

        var stateType = moveDelta.sqrMagnitude > 0f ? PixemAnimationType.Run : PixemAnimationType.Idle;
        if (moveDelta.x != 0f)
        {
            executionContext.ModelProcessor.SetFlip(moveDelta.x > 0f);
        }

        executionContext.ModelProcessor.SetStateType(stateType);
    }

    void RefreshControlledCache(IControlled controlled)
    {
        executionContext?.TryInitialize(controlled);
    }

    void ClearControlledCache(IControlled controlled)
    {
        if (executionContext == null || !executionContext.Matches(controlled))
        {
            return;
        }

        ResetControlledCache();
    }

    void ResetControlledCache()
    {
        executionContext?.Reset();
    }
}
